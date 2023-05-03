using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class NpcThinkBehaviour : Behaviour
    {
        private NonTargetAction[] nonTargetActions;

        private NpcEventHandler npcEventHandler;

        public NpcThinkBehaviour(NonTargetAction[] nonTargetActions, NpcEventHandler npcEventHandler)
        {
            this.nonTargetActions = nonTargetActions;

            this.npcEventHandler = npcEventHandler;
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

            playerSay = server.GameObjectEventHandlers.Subscribe<PlayerSayEventArgs>(GameObject, async (context, e) =>
            {
                if (npc.Tile.Position.IsInRange(e.Player.Tile.Position, 3) )
                {
                    if (queue.Count == 0)
                    {
                        if (e.Message == "hi" || e.Message == "hello")
                        {
                            queue.Add(e.Player);

                            await npcEventHandler.OnGreet(npc, e.Player);

                            await Context.AddCommand(new CreatureUpdateDirectionCommand(npc, npc.Tile.Position.ToDirection(e.Player.Tile.Position).Value) );
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
                                    await npcEventHandler.OnGreet(npc, queue.Peek() );

                                    await Context.AddCommand(new CreatureUpdateDirectionCommand(npc, npc.Tile.Position.ToDirection(queue.Peek().Tile.Position).Value) );
                                }
                                else
                                {
                                    await npcEventHandler.OnFarewell(npc, e.Player);
                                }
                            }
                            else
                            {
                                await npcEventHandler.OnSay(npc, e.Player, e.Message);
                            }
                        }
                        else
                        {
                            if (e.Message == "hi" || e.Message == "hello")
                            {
                                queue.Add(e.Player);

                                await npcEventHandler.OnGreetBusy(npc, e.Player);
                            }
                        }
                    }
                }
            } );

            //TODO: Use local event

            creatureWalk = server.EventHandlers.Subscribe<CreatureWalkEventArgs>(async (context, e) =>
            {
                if (e.Creature is Player player)
                {
                    if (npc.Tile.Position.IsInRange(e.ToTile.Position, 3) )
                    {
                        if (queue.Peek() == e.Creature)
                        {
                            await Context.AddCommand(new CreatureUpdateDirectionCommand(npc, npc.Tile.Position.ToDirection(e.Creature.Tile.Position).Value) );
                        }
                    }
                    else
                    {
                        if (queue.Peek() == e.Creature)
                        {
                            queue.Remove(player);

                            if (queue.Count > 0)
                            {
                                await npcEventHandler.OnGreet(npc, queue.Peek() );

                                await Context.AddCommand(new CreatureUpdateDirectionCommand(npc, npc.Tile.Position.ToDirection(queue.Peek().Tile.Position).Value) );
                            }
                            else
                            {
                                await npcEventHandler.OnDisappear(npc);
                            }
                        }
                        else
                        {
                            queue.Remove(player);
                        }
                    }
                }
            } );

            //TODO: Use local event

            playerLogout = server.EventHandlers.Subscribe<PlayerLogoutEventArgs>(async (context, e) =>
            {
                if (queue.Peek() == e.Player)
                {
                    queue.Remove(e.Player);

                    if (queue.Count > 0)
                    {
                        await npcEventHandler.OnGreet(npc, queue.Peek() );

                        await Context.AddCommand(new CreatureUpdateDirectionCommand(npc, npc.Tile.Position.ToDirection(queue.Peek().Tile.Position).Value) );
                    }
                    else
                    {
                        await npcEventHandler.OnDisappear(npc);
                    }
                }
                else
                {
                    queue.Remove(e.Player);
                }
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