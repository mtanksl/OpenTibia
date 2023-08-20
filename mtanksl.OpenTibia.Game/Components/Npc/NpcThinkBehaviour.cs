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
                if (npc.Tile.Position.IsInRange(e.Player.Tile.Position, 3) )
                {
                    if (targets.Count == 0)
                    {
                        if (e.Message == "hi" || e.Message == "hello")
                        {
                            targets.Add(e.Player);

                            return conversationStrategy.Greeting(npc, e.Player);
                        }
                    }
                    else
                    {
                        if (e.Player != targets.Peek() )
                        {
                            if (e.Message == "hi" || e.Message == "hello")
                            {
                                targets.Add(e.Player);

                                return conversationStrategy.Busy(npc, e.Player);
                            }
                        }
                        else
                        {
                            if (e.Message == "bye" || e.Message == "farewell")
                            {
                                targets.Remove(e.Player);

                                return conversationStrategy.Farewell(npc, e.Player);
                            }

                            return conversationStrategy.Say(npc, e.Player, e.Message);
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