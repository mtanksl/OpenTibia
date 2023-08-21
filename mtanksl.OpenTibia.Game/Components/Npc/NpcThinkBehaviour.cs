using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class NpcThinkBehaviour : Behaviour
    {
        private IConversationStrategy conversationStrategy;

        private IWalkStrategy walkStrategy;

        public NpcThinkBehaviour(IConversationStrategy conversationStrategy, IWalkStrategy walkStrategy)
        {
            this.conversationStrategy = conversationStrategy;

            this.walkStrategy = walkStrategy;
        }

        private Guid playerSay;

        private Guid globalTick;

        public override void Start()
        {
            Npc npc = (Npc)GameObject;

            QueueHashSet<Player> targets = new QueueHashSet<Player>();

            playerSay = Context.Server.EventHandlers.Subscribe<PlayerSayEventArgs>( (context, e) =>
            {
                Player player = e.Player;

                if (npc.Tile.Position.IsInRange(player.Tile.Position, 3) )
                {               
                    string message = e.Message;

                    if (targets.Count == 0)
                    {
                        if (message == "hi" || message == "hello")
                        {
                            targets.Add(player);

                            return conversationStrategy.Greeting(npc, player);
                        }
                    }
                    else
                    {
                        if (player != targets.Peek() )
                        {
                            if (message == "hi" || message == "hello")
                            {
                                targets.Add(player);

                                return conversationStrategy.Busy(npc, player);
                            }
                        }
                        else
                        {
                            if (message == "bye" || message == "farewell")
                            {
                                targets.Remove(player);

                                while (targets.Count > 0)
                                {
                                    Player next = targets.Peek();

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
                                    Player next = targets.Peek();

                                    return conversationStrategy.Greeting(npc, next);
                                }
                                   
                                return conversationStrategy.Farewell(npc, player);
                            }
                                
                            return conversationStrategy.Say(npc, player, message);
                        }
                    }
                }

                return Promise.Completed;
            } );

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
            {
                if (targets.Count == 0)
                {
                    CreatureWalkBehaviour creatureWalkBehaviour = Context.Server.GameObjectComponents.GetComponent<CreatureWalkBehaviour>(npc);

                    if (creatureWalkBehaviour == null)
                    {
                        Context.Server.GameObjectComponents.AddComponent(npc, new CreatureWalkBehaviour(walkStrategy, null) );
                    }

                    CreatureFocusBehaviour creatureFocusBehaviour = Context.Server.GameObjectComponents.GetComponent<CreatureFocusBehaviour>(npc);

                    if (creatureFocusBehaviour != null)
                    {
                        Context.Server.GameObjectComponents.RemoveComponent(npc, creatureFocusBehaviour);
                    }
                }
                else
                {
                    CreatureWalkBehaviour creatureWalkBehaviour = Context.Server.GameObjectComponents.GetComponent<CreatureWalkBehaviour>(npc);

                    if (creatureWalkBehaviour != null)
                    {
                        Context.Server.GameObjectComponents.RemoveComponent(npc, creatureWalkBehaviour);
                    }

                    CreatureFocusBehaviour creatureFocusBehaviour = Context.Server.GameObjectComponents.GetComponent<CreatureFocusBehaviour>(npc);

                    if (creatureFocusBehaviour == null || creatureFocusBehaviour.Target != targets.Peek() )
                    {
                        Context.Server.GameObjectComponents.AddComponent(npc, new CreatureFocusBehaviour(targets.Peek() ) );
                    }
                }

                if (targets.Count > 0)
                {
                    var player = targets.Peek();

                    if (player.Tile == null || player.IsDestroyed || !npc.Tile.Position.IsInRange(player.Tile.Position, 3) )
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
			                var next = targets.Peek();

			                return conversationStrategy.Greeting(npc, next);
		                }

                        return conversationStrategy.Dismiss(npc, player);
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