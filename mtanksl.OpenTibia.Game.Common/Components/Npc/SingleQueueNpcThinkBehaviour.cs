using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Game.Components
{
    public class SingleQueueNpcThinkBehaviour : Behaviour
    {
        private IWalkStrategy idleWalkStrategy;

        public SingleQueueNpcThinkBehaviour(IWalkStrategy idleWalkStrategy)
        {
            this.idleWalkStrategy = idleWalkStrategy;
        }

        private QueueHashSet<Player> queue = new QueueHashSet<Player>();

        private DateTime lastSay;

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
                await dialoguePlugin.OnDequeue(npc, player);
            }

            while (queue.Count > 0)
            {
                Player next = queue.Peek();

                if (next.Tile == null || next.IsDestroyed || !npc.Tile.Position.IsInRange(next.Tile.Position, 3) )
                {
                    if (queue.Remove(next) )
                    {
                        await dialoguePlugin.OnDequeue(npc, next);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void Reload()
        {
            dialoguePlugin = Context.Server.Plugins.GetDialoguePlugin(npc.Name) ?? Context.Server.Plugins.GetDialoguePlugin("");

            queue.Clear();
        }

        private Npc npc;

        private DialoguePlugin dialoguePlugin;

        private Guid globalServerReloaded;

        private int ticks;

        private Guid globalTick;

        private Guid playerSay;

        private Guid playerSayToNpc;

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
        }

        private async Promise OnThink(Context context, GlobalTickEventArgs e)
        {
            ticks -= e.Ticks;

            while (ticks <= 0)
            {
                ticks += 1000;

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

                    if (target.Tile == null || target.IsDestroyed || !npc.Tile.Position.IsInRange(target.Tile.Position, 3) || (DateTime.UtcNow - lastSay).TotalMinutes >= 1)
                    {
                        await Disappear(target);
                    }
                    else
                    {
                        Direction? direction = npc.Tile.Position.ToDirection(target.Tile.Position);

                        if (direction != null)
                        {
                            await Context.AddCommand(new CreatureUpdateDirectionCommand(npc, direction.Value) );
                        }
                    }
                }
            }
        }

        private async Promise Say(Player player, string message)
        {
            if (npc.Tile.Position.IsInRange(player.Tile.Position, 3) )
            {               
                if (queue.Count == 0)
                {
                    if (await dialoguePlugin.ShouldGreet(npc, player, message) )
                    {
                        await Add(player);

                        lastSay = DateTime.UtcNow;

                        await dialoguePlugin.OnGreet(npc, player);
                    }
                }
                else
                {
                    if (player != queue.Peek() )
                    {
                        if (await dialoguePlugin.ShouldGreet(npc, player, message) )
                        {
                            await Add(player);

                            await dialoguePlugin.OnBusy(npc, player);
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
                            lastSay = DateTime.UtcNow;

                            await dialoguePlugin.OnSay(npc, player, message);
                        }
                    }
                }
            }
        }

        public async Promise Idle(Player player)
        {
            await Remove(player);

            if (queue.Count > 0)
            {
                Player next = queue.Peek();

                lastSay = DateTime.UtcNow;

                await dialoguePlugin.OnGreet(npc, next);
            }
        }

        public async Promise Farewell(Player player)
        {
            await Remove(player);

            if (queue.Count > 0)
            {
                Player next = queue.Peek();

                lastSay = DateTime.UtcNow;

                await dialoguePlugin.OnGreet(npc, next);
            }
            else
            {
                await dialoguePlugin.OnFarewell(npc, player);
            }
        }

        public async Promise Disappear(Player player)
        {
            await Remove(player);

            if (queue.Count > 0)
            {
                Player next = queue.Peek();

                lastSay = DateTime.UtcNow;

                await dialoguePlugin.OnGreet(npc, next);
            }
            else
            {
                await dialoguePlugin.OnDisappear(npc, player);
            }
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe(globalServerReloaded);

            Context.Server.EventHandlers.Unsubscribe(globalTick);

            Context.Server.GameObjectEventHandlers.Unsubscribe(playerSay);

            Context.Server.GameObjectEventHandlers.Unsubscribe(playerSayToNpc);            
        }
    }
}