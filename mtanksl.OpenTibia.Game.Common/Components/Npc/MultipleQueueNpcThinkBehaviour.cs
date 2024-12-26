using OpenTibia.Common.Objects;
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
        private DialoguePlugin dialoguePlugin;

        private IWalkStrategy walkStrategy;

        public MultipleQueueNpcThinkBehaviour(IWalkStrategy walkStrategy)
        {
            this.walkStrategy = walkStrategy;
        }

        private QueueHashSet<Player> queue = new QueueHashSet<Player>();

        private async Promise Add(Player player)
        {
            Npc npc = (Npc)GameObject;

            if (queue.Add(player) )
            {
                await dialoguePlugin.OnEnqueue(npc, player);
            }
        }

        private async Promise Remove(Player player)
        {
            Npc npc = (Npc)GameObject;

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

        private void Clear()
        {
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

        public async Promise Buy(Player player, ushort openTibiaId, byte type, byte count, int price, bool ignoreCapacity, bool buyWithBackpacks)
        {
            Npc npc = (Npc)GameObject;

            await dialoguePlugin.OnBuy(npc, player, openTibiaId, type, count, price, ignoreCapacity, buyWithBackpacks);
        }

        public async Promise Sell(Player player, ushort openTibiaId, byte type, byte count, int price, bool keepEquipped)
        { 
            Npc npc = (Npc)GameObject;

            await dialoguePlugin.OnSell(npc, player, openTibiaId, type, count, price, keepEquipped);
        }

        public async Promise CloseNpcTrade(Player player)
        {
            Npc npc = (Npc)GameObject;

            await dialoguePlugin.OnCloseNpcTrade(npc, player);
        }

        public async Promise CloseNpcsChannel(Player player)
        {
            Npc npc = (Npc)GameObject;

            await Remove(player);
        }

        public async Promise Idle(Player player)
        {
            Npc npc = (Npc)GameObject;

            await Remove(player);
        }

        public async Promise Farewell(Player player)
        {
            Npc npc = (Npc)GameObject;

            await Remove(player);

            await dialoguePlugin.OnFarewell(npc, player);
        }

        public async Promise Disappear(Player player)
        {
            Npc npc = (Npc)GameObject;

            await Remove(player);

            await dialoguePlugin.OnDisappear(npc, player);
        }

        private Guid globalServerReloaded;

        private Guid playerSay;

        private Guid playerSayToNpc;

        private Guid globalTick;

        public override void Start()
        {
            Npc npc = (Npc)GameObject;

            dialoguePlugin = Context.Server.Plugins.GetDialoguePlugin(npc.Name) ?? Context.Server.Plugins.GetDialoguePlugin("Default");

            globalServerReloaded = Context.Server.EventHandlers.Subscribe<GlobalServerReloadedEventArgs>( (context, e) =>
            {
                Clear();

                dialoguePlugin = Context.Server.Plugins.GetDialoguePlugin(npc.Name) ?? Context.Server.Plugins.GetDialoguePlugin("Default");

                return Promise.Completed;
            } );

            playerSay = Context.Server.GameObjectEventHandlers.Subscribe<PlayerSayEventArgs>(GameObject, (context, e) => Say(e.Player, e.Message) );

            playerSayToNpc = Context.Server.GameObjectEventHandlers.Subscribe<PlayerSayToNpcEventArgs>(GameObject, (context, e) => Say(e.Player, e.Message) );

            globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance[npc.Id % GlobalTickEventArgs.Instance.Length], async (context, e) =>
            {
                foreach (var player in queue.ToList() )
                {
                    if (player.Tile == null || player.IsDestroyed || !npc.Tile.Position.IsInRange(player.Tile.Position, 3) )
                    {
                        await Disappear(player);
                    }
                }

                if (queue.Count == 0)
                {
                    if (walkStrategy != null)
                    {
                        Tile toTile;

                        if (walkStrategy.CanWalk(npc, null, out toTile) )
                        {
                            await Context.Current.AddCommand(new CreatureMoveCommand(npc, toTile) );
                        }
                    }
                }
                else
                {
                    Player target = queue.Peek();

                    Direction? direction = npc.Tile.Position.ToDirection(target.Tile.Position);

                    if (direction != null)
                    {
                        await Context.AddCommand(new CreatureUpdateDirectionCommand(npc, direction.Value) );
                    }
                }                
            } );

            async Promise Say(Player player, string message)
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
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe(globalServerReloaded);

            Context.Server.GameObjectEventHandlers.Unsubscribe(GameObject, playerSay);

            Context.Server.GameObjectEventHandlers.Unsubscribe(GameObject, playerSayToNpc);

            Context.Server.EventHandlers.Unsubscribe(globalTick);
        }
    }
}