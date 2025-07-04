﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Events;
using OpenTibia.Game.Plugins;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class MultipleQueueNpcThinkBehaviour : Behaviour
    {
        private IWalkStrategy idleWalkStrategy;

        public MultipleQueueNpcThinkBehaviour(IWalkStrategy idleWalkStrategy)
        {
            this.idleWalkStrategy = idleWalkStrategy;
        }

        private QueueHashSet<Player> queue = new QueueHashSet<Player>();

        private async Promise Add(Player player)
        {
            if (queue.Add(player) )
            {
                await dialoguePlugin.OnEnqueue(npc, player);
            }
        }

        private async Promise Remove(Player player)
        {
            if (queue.Remove(player) )
            {
                NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(player);

                if (trading != null)
                {
                    Context.Server.NpcTradings.RemoveTrading(trading);

                    Context.AddPacket(trading.CounterOfferPlayer, new CloseNpcTradeOutgoingPacket() );
                }

                await dialoguePlugin.OnDequeue(npc, player);
            }
        }

        private void Reload()
        {
            dialoguePlugin = Context.Server.Plugins.GetDialoguePlugin(npc.Name) ?? Context.Server.Plugins.GetDialoguePlugin("");

            foreach (var player in queue)
            {
                NpcTrading trading = Context.Server.NpcTradings.GetTradingByCounterOfferPlayer(player);

                if (trading != null)
                {
                    Context.Server.NpcTradings.RemoveTrading(trading);

                    Context.AddPacket(trading.CounterOfferPlayer, new CloseNpcTradeOutgoingPacket() );
                }
            }

            queue.Clear();
        }

        private Npc npc;

        private DialoguePlugin dialoguePlugin;

        private Guid globalServerReloaded;

        private int ticks;

        private Guid globalTick;

        private Guid playerSay;

        private Guid playerSayToNpc;

        private Guid playerBuyNpcTrade;

        private Guid playerSellNpcTrade;

        private Guid playerCloseNpcTrade;

        private Guid playerCloseNpcsChannel;

        private DateTime nextWalk;

        public override void Start()
        {
            npc = (Npc)GameObject;

            Reload();

            globalServerReloaded = Context.Server.EventHandlers.Subscribe<GlobalServerReloadedEventArgs>( (context, e) =>
            {
                Reload();

                return Promise.Completed;
            } );

            ticks = 1000;

            globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance(npc.Id), OnThink);

            playerSay = Context.Server.GameObjectEventHandlers.Subscribe<ObserveEventArgs<PlayerSayEventArgs> >(npc, (context, e) => Say(e.SourceEvent.Player, e.SourceEvent.Message) );

            playerSayToNpc = Context.Server.GameObjectEventHandlers.Subscribe<ObserveEventArgs<PlayerSayToNpcEventArgs> >(npc, (context, e) => Say(e.SourceEvent.Player, e.SourceEvent.Message) );

            playerBuyNpcTrade = Context.Server.GameObjectEventHandlers.Subscribe<ObserveEventArgs<PlayerBuyNpcTradeEventArgs> >(npc, (context, e) => Buy(e.SourceEvent.Player, e.SourceEvent.OpenTibiaId, e.SourceEvent.Type, e.SourceEvent.Count, e.SourceEvent.Price, e.SourceEvent.IgnoreCapacity, e.SourceEvent.BuyWithBackpacks) );

            playerSellNpcTrade = Context.Server.GameObjectEventHandlers.Subscribe<ObserveEventArgs<PlayerSellNpcTradeEventArgs> >(npc, (context, e) => Sell(e.SourceEvent.Player, e.SourceEvent.OpenTibiaId, e.SourceEvent.Type, e.SourceEvent.Count, e.SourceEvent.Price, e.SourceEvent.KeepEquipped) );

            playerCloseNpcTrade = Context.Server.GameObjectEventHandlers.Subscribe<ObserveEventArgs<PlayerCloseNpcTradeEventArgs> >(npc, (context, e) => CloseNpcTrade(e.SourceEvent.Player) );

            playerCloseNpcsChannel = Context.Server.GameObjectEventHandlers.Subscribe<ObserveEventArgs<PlayerCloseNpcsChannelEventArgs> >(npc, (context, e) => CloseNpcsChannel(e.SourceEvent.Player) );
        }

        private async Promise OnThink(Context context, GlobalTickEventArgs e)
        {
            ticks -= e.Ticks;

            while (ticks <= 0)
            {
                ticks += 1000;

                foreach (var player in queue.ToArray() )
                {
                    if (player.Tile == null || player.IsDestroyed || !npc.Tile.Position.IsInRange(player.Tile.Position, 3) )
                    {
                        await Disappear(player);
                    }
                }

                if (queue.Count == 0)
                {
                    if (DateTime.UtcNow >= nextWalk)
                    {
                        Tile toTile;

                        if (idleWalkStrategy.CanWalk(npc, null, out toTile) )
                        {                           
                            MoveDirection moveDirection = npc.Tile.Position.ToMoveDirection(toTile.Position).Value;

                            await Context.AddCommand(new CreatureMoveCommand(npc, toTile) );

                            nextWalk = DateTime.UtcNow.AddMilliseconds(Formula.GetStepDuration(npc, toTile, moveDirection) );
                        }
                        else
                        {
                            nextWalk = DateTime.UtcNow.AddSeconds(1);
                        }

                        nextWalk = nextWalk.AddSeconds(1);
                    }
                }
                else
                {
                    Player target = queue.Peek();

                    Direction? direction = npc.Tile.Position.ToDirection(target.Tile.Position);

                    if (direction != null)
                    {
                        await Context.AddCommand(new CreatureUpdateDirectionCommand(npc, direction.Value));
                    }
                }
            }
        }

        private async Promise Say(Player player, string message)
        {
            if (npc.Tile.Position.IsInRange(player.Tile.Position, 3) )
            {               
                if ( !queue.Contains(player) )
                {
                    if (await dialoguePlugin.ShouldGreet(npc, player, message) )
                    {
                        await Add(player);

                        await dialoguePlugin.OnGreet(npc, player);
                    }
                }
                else
                {
                    if (await dialoguePlugin.ShouldFarewell(npc, player, message) )
                    {
                        await Farewell(player);
                    }
                    else
                    {
                        await dialoguePlugin.OnSay(npc, player, message);
                    }
                }
            }
        }

        private async Promise Buy(Player player, ushort openTibiaId, byte type, byte count, int price, bool ignoreCapacity, bool buyWithBackpacks /* unused */)
        {
            if (ignoreCapacity)
            {
                await dialoguePlugin.OnBuy(npc, player, openTibiaId, type, count, price, ignoreCapacity, buyWithBackpacks);
            }
            else
            {
                ItemMetadata itemMetadata = Context.Server.ItemFactory.GetItemMetadataByOpenTibiaId(openTibiaId);

                uint weight;

                if (itemMetadata.Flags.Is(ItemMetadataFlags.Stackable) )
                {
                    weight = type * (itemMetadata.Weight ?? 0);
                }
                else
                {
                    weight = (itemMetadata.Weight ?? 0);
                }

                weight *= count;

                uint capacity = player.Capacity;

                if (weight <= capacity)
                {
                    await dialoguePlugin.OnBuy(npc, player, openTibiaId, type, count, price, ignoreCapacity, buyWithBackpacks);
                }
            }
        }

        private async Promise Sell(Player player, ushort openTibiaId, byte type, byte count, int price, bool keepEquipped /* unused */)
        { 
            await dialoguePlugin.OnSell(npc, player, openTibiaId, type, count, price, keepEquipped);
        }

        private async Promise CloseNpcTrade(Player player)
        {
            await dialoguePlugin.OnCloseNpcTrade(npc, player);
        }

        private async Promise CloseNpcsChannel(Player player)
        {
            await Remove(player);
        }

        public async Promise Idle(Player player)
        {
            await Remove(player);
        }

        public async Promise Farewell(Player player)
        {
            await Remove(player);

            await dialoguePlugin.OnFarewell(npc, player);
        }

        public async Promise Disappear(Player player)
        {
            await Remove(player);

            await dialoguePlugin.OnDisappear(npc, player);
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe(globalServerReloaded);

            Context.Server.EventHandlers.Unsubscribe(globalTick);

            Context.Server.GameObjectEventHandlers.Unsubscribe(playerSay);

            Context.Server.GameObjectEventHandlers.Unsubscribe(playerSayToNpc);

            Context.Server.GameObjectEventHandlers.Unsubscribe(playerBuyNpcTrade);

            Context.Server.GameObjectEventHandlers.Unsubscribe(playerSellNpcTrade);

            Context.Server.GameObjectEventHandlers.Unsubscribe(playerCloseNpcTrade);

            Context.Server.GameObjectEventHandlers.Unsubscribe(playerCloseNpcsChannel);
        }
    }
}