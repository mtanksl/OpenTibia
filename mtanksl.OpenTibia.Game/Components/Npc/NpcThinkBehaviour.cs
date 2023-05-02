using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class NpcThinkBehaviour : Behaviour
    {
        private NonTargetAction[] nonTargetActions;

        private NpcScript npcScript;

        public NpcThinkBehaviour(NonTargetAction[] nonTargetActions, NpcScript npcScript)
        {
            this.nonTargetActions = nonTargetActions;

            this.npcScript = npcScript;
        }

        private Guid globalTick;

        private Guid playerSay;

        private Guid creatureWalk;

        private Guid playerLogout;

        public override void Start(Server server)
        {
            Npc npc = (Npc)GameObject;

            QueueHashSet<Player> queue = new QueueHashSet<Player>();

            globalTick = server.EventHandlers.Subscribe<GlobalTickEventArgs>(async (context, e) =>
            {
                if (queue.Count == 0)
                {
                    foreach (var nonTargetAction in nonTargetActions)
                    {
                        await nonTargetAction.Update(npc);
                    }
                }
            } );

            playerSay = server.GameObjectEventHandlers.Subscribe<PlayerSayEventArgs>(GameObject, (context, e) =>
            {
                if (npc.Tile.Position.IsInRange(e.Player.Tile.Position, 3) )
                {
                    if (queue.Count == 0)
                    {
                        if (e.Message == "hi" || e.Message == "hello")
                        {
                            Context.AddCommand(new NpcSayCommand(npc, npcScript.Address(npc, e.Player) ) );

                            Context.AddCommand(new CreatureUpdateDirectionCommand(npc, npc.Tile.Position.ToDirection(e.Player.Tile.Position).Value) );

                            queue.Add(e.Player);
                        }
                    }
                    else
                    {
                        if (queue.Peek() == e.Player)
                        {
                            if (e.Message == "bye" || e.Message == "farewell")
                            {
                                queue.Remove(e.Player);

                                if (queue.Count > 0)
                                {
                                    Context.AddCommand(new NpcSayCommand(npc, npcScript.Address(npc, queue.Peek() ) ) );

                                    Context.AddCommand(new CreatureUpdateDirectionCommand(npc, npc.Tile.Position.ToDirection(queue.Peek().Tile.Position).Value) );
                                }
                                else
                                {
                                    Context.AddCommand(new NpcSayCommand(npc, npcScript.Idle(npc, e.Player) ) );
                                }
                            }
                            else
                            {
                                var answer = npcScript.Handle(npc, e.Player, e.Message);

                                if (answer != null)
                                {
                                    Context.AddCommand(new NpcSayCommand(npc, answer) );
                                }
                            }
                        }
                        else
                        {
                            if (e.Message == "hi" || e.Message == "hello")
                            {
                                Context.AddCommand(new NpcSayCommand(npc, npcScript.Busy(npc, e.Player) ) );

                                queue.Add(e.Player);
                            }
                        }
                    }
                }

                return Promise.Completed;
            } );

            //TODO: Use local event

            creatureWalk = server.EventHandlers.Subscribe<CreatureWalkEventArgs>( (context, e) =>
            {
                if (e.Creature is Player player)
                {
                    if (npc.Tile.Position.IsInRange(e.ToTile.Position, 3) )
                    {
                        if (queue.Peek() == e.Creature)
                        {
                            Context.AddCommand(new CreatureUpdateDirectionCommand(npc, npc.Tile.Position.ToDirection(e.Creature.Tile.Position).Value) );
                        }
                    }
                    else
                    {
                        if (queue.Peek() == e.Creature)
                        {
                            queue.Remove(player);

                            if (queue.Count > 0)
                            {
                                Context.AddCommand(new NpcSayCommand(npc, npcScript.Address(npc, queue.Peek() ) ) );

                                Context.AddCommand(new CreatureUpdateDirectionCommand(npc, npc.Tile.Position.ToDirection(queue.Peek().Tile.Position).Value) );
                            }
                            else
                            {
                                Context.AddCommand(new NpcSayCommand(npc, npcScript.Vanish(npc) ) );
                            }
                        }
                        else
                        {
                            queue.Remove(player);
                        }
                    }
                }

                return Promise.Completed;
            } );

            //TODO: Use local event

            playerLogout = server.EventHandlers.Subscribe<PlayerLogoutEventArgs>( (context, e) =>
            {
                if (queue.Peek() == e.Player)
                {
                    queue.Remove(e.Player);

                    if (queue.Count > 0)
                    {
                        Context.AddCommand(new NpcSayCommand(npc, npcScript.Address(npc, queue.Peek() ) ) );

                        Context.AddCommand(new CreatureUpdateDirectionCommand(npc, npc.Tile.Position.ToDirection(queue.Peek().Tile.Position).Value) );
                    }
                    else
                    {
                        Context.AddCommand(new NpcSayCommand(npc, npcScript.Vanish(npc) ) );
                    }
                }
                else
                {
                    queue.Remove(e.Player);
                }

                return Promise.Completed;
            } );
        }

        public override void Stop(Server server)
        {
            server.EventHandlers.Unsubscribe<GlobalTickEventArgs>(globalTick);

            server.GameObjectEventHandlers.Unsubscribe<PlayerSayEventArgs>(GameObject, playerSay);

            server.EventHandlers.Unsubscribe<CreatureWalkEventArgs>(creatureWalk);

            server.EventHandlers.Unsubscribe<PlayerLogoutEventArgs>(playerLogout);
        }
    }
}