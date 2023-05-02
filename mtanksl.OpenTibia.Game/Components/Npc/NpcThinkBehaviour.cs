using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class NpcThinkBehaviour : Behaviour
    {
        private BehaviourAction[] actions;

        public NpcThinkBehaviour(BehaviourAction[] actions)
        {
            this.actions = actions;
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
                    foreach (var action in actions)
                    {
                        await action.Update(npc, null);
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
                            Context.AddCommand(new NpcSayCommand(npc, "Hello, " + e.Player.Name + "! Feel free to ask me for help.") );

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
                                    Context.AddCommand(new NpcSayCommand(npc, "Hello, " + queue.Peek().Name + "! Feel free to ask me for help.") );

                                    Context.AddCommand(new CreatureUpdateDirectionCommand(npc, npc.Tile.Position.ToDirection(queue.Peek().Tile.Position).Value) );
                                }
                                else
                                {
                                    Context.AddCommand(new NpcSayCommand(npc, "Farewell, " + e.Player.Name + "!") );
                                }
                            }
                            else if (e.Message == "name")
                            {
                                Context.AddCommand(new NpcSayCommand(npc, "My name is Cipfried.") );
                            }
                            else if (e.Message == "job")
                            {
                                Context.AddCommand(new NpcSayCommand(npc, "I am just a humble monk. Ask me if you need help or healing.") );
                            }
                            else if (e.Message == "monk")
                            {
                                Context.AddCommand(new NpcSayCommand(npc, "I sacrifice my life to serve the good gods of Tibia.") );
                            }
                            else if (e.Message == "tibia")
                            {
                                Context.AddCommand(new NpcSayCommand(npc, "That's where we are. The world of Tibia.") );
                            }
                            else
                            {
                                //...
                            }
                        }
                        else
                        {
                            if (e.Message == "hi" || e.Message == "hello")
                            {
                                Context.AddCommand(new NpcSayCommand(npc, "Please wait, " + e.Player.Name + ". I already talk to someone!") );

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
                        queue.Remove( (Player)e.Creature);

                        if (queue.Count > 0)
                        {
                            Context.AddCommand(new NpcSayCommand(npc, "Hello, " + queue.Peek().Name + "! Feel free to ask me for help.") );

                            Context.AddCommand(new CreatureUpdateDirectionCommand(npc, npc.Tile.Position.ToDirection(queue.Peek().Tile.Position).Value) );
                        }
                        else
                        {
                            Context.AddCommand(new NpcSayCommand(npc, "Well, bye then.") );
                        }
                    }
                    else
                    {
                        queue.Remove( (Player)e.Creature);
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
                        Context.AddCommand(new NpcSayCommand(npc, "Hello, " + queue.Peek().Name + "! Feel free to ask me for help.") );

                        Context.AddCommand(new CreatureUpdateDirectionCommand(npc, npc.Tile.Position.ToDirection(queue.Peek().Tile.Position).Value) );
                    }
                    else
                    {
                        Context.AddCommand(new NpcSayCommand(npc, "Well, bye then.") );
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