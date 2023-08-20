using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class NpcThinkBehaviour : Behaviour
    {
        private IConversationStrategy conversationStrategy;

        public NpcThinkBehaviour(IConversationStrategy conversationStrategy)
        {
            this.conversationStrategy = conversationStrategy;
        }

        private Guid playerSay;

        private Guid globalTick;

        public override void Start()
        {
            Npc npc = (Npc)GameObject;

            DateTime lastSentence = DateTime.UtcNow;

            QueueHashSet<Player> targets = new QueueHashSet<Player>();

            playerSay = Context.Server.EventHandlers.Subscribe<PlayerSayEventArgs>( (context, e) =>
            {
                var player = e.Player;

                if (npc.Tile.Position.IsInRange(player.Tile.Position, 3) )
                {
                    if (targets.Count == 0)
                    {
                        if (e.Message == "hi" || e.Message == "hello")
                        {
                            lastSentence = DateTime.UtcNow;

                            targets.Add(player);

                            return conversationStrategy.Greeting(npc, player);
                        }
                    }
                    else
                    {
                        if (player == targets.Peek() )
                        {
                            if (e.Message == "bye" || e.Message == "farewell")
                            {
                                targets.Remove(player);

                                while (targets.Count > 0)
                                {
                                    var next = targets.Peek();

                                    if (next.Tile == null || next.IsDestroyed || !npc.Tile.Position.IsInRange(next.Tile.Position, 3) )
                                    {
                                        targets.Remove(next);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                if (targets.Count > 0)
                                {
                                    lastSentence = DateTime.UtcNow;

                                    var next = targets.Peek();

                                    return conversationStrategy.Greeting(npc, next);
                                }
                                else
                                {
                                    return conversationStrategy.Farewell(npc, player);
                                }
                            }
                            else
                            {
                                lastSentence = DateTime.UtcNow;

                                return conversationStrategy.Say(npc, player, e.Message);
                            }
                        }
                        else
                        {
                            if (e.Message == "hi" || e.Message == "hello")
                            {
                                targets.Add(player);

                                return conversationStrategy.Busy(npc, player);
                            }
                        }
                    }
                }

                return Promise.Completed;
            } );

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
            {
                if (targets.Count > 0)
                {
                    var player = targets.Peek();

                    if (player.Tile == null || player.IsDestroyed || !npc.Tile.Position.IsInRange(player.Tile.Position, 3) || (DateTime.UtcNow - lastSentence).TotalSeconds > 15)
                    {
                        targets.Remove(player);

                        while (targets.Count > 0)
                        {
                            var next = targets.Peek();

                            if (next.Tile == null || next.IsDestroyed || !npc.Tile.Position.IsInRange(next.Tile.Position, 3) )
                            {
                                targets.Remove(next);
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (targets.Count > 0)
                        {
                            lastSentence = DateTime.UtcNow;

                            var next = targets.Peek();

                            return conversationStrategy.Greeting(npc, next);
                        }
                        else
                        {
                            return conversationStrategy.Dismiss(npc, player);
                        }
                    }
                    else
                    {
                        var direction = npc.Tile.Position.ToDirection(player.Tile.Position);

                        if (direction != null && direction != npc.Direction)
                        {
                            return Context.AddCommand(new CreatureUpdateDirectionCommand(npc, direction.Value) );
                        }
                    }
                }
                                            
                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe<PlayerSayEventArgs>(playerSay);

            Context.Server.EventHandlers.Unsubscribe<GlobalTickEventArgs>(globalTick);
        }
    }
}