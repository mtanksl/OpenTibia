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
        private DialoguePlugin dialoguePlugin;

        private IWalkStrategy walkStrategy;

        public SingleQueueNpcThinkBehaviour(IWalkStrategy walkStrategy)
        {
            this.walkStrategy = walkStrategy;
        }

        private QueueHashSet<Player> queue = new QueueHashSet<Player>();

        private DateTime lastSay;

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

        private void Clear()
        {
            queue.Clear();
        }

        public async Promise Idle(Player player)
        {
            Npc npc = (Npc)GameObject;

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
            Npc npc = (Npc)GameObject;

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
            Npc npc = (Npc)GameObject;

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

        private Guid globalServerReloaded;

        private Guid playerSay;

        private Guid playerSayToNpc;

        private Guid globalTick;

        private DateTime nextWalk = DateTime.MinValue;

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
                if (queue.Count == 0)
                {
                    if (DateTime.UtcNow >= nextWalk)
                    {
                        if (walkStrategy != null)
                        {
                            Tile toTile;

                            if (walkStrategy.CanWalk(npc, null, out toTile) )
                            {
                                nextWalk = DateTime.UtcNow.AddMilliseconds(1000 * toTile.Ground.Metadata.Speed / npc.Speed);

                                await Context.Current.AddCommand(new CreatureMoveCommand(npc, toTile) );
                            }
                        }
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
            } );

            async Promise Say(Player player, string message)
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