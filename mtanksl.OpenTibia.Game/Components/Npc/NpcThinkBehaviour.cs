using OpenTibia.Common.Objects;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class NpcThinkBehaviour : Behaviour
    {
        private DialoguePlugin dialoguePlugin;

        private IWalkStrategy walkStrategy;

        public NpcThinkBehaviour(DialoguePlugin dialoguePlugin, IWalkStrategy walkStrategy)
        {
            this.dialoguePlugin = dialoguePlugin;

            this.walkStrategy = walkStrategy;
        }

        private Guid playerSay;

        private Guid globalTick;

        public override void Start()
        {
            Npc npc = (Npc)GameObject;

            QueueHashSet<Player> queue = new QueueHashSet<Player>();

            playerSay = Context.Server.EventHandlers.Subscribe<PlayerSayEventArgs>(async (context, e) =>
            {
                Player player = e.Player;

                if (npc.Tile.Position.IsInRange(player.Tile.Position, 3) )
                {               
                    string message = e.Message;

                    if (queue.Count == 0)
                    {
                        if (await dialoguePlugin.ShouldGreet(npc, player, message) )
                        {
                            queue.Add(player);

                            await dialoguePlugin.OnGreet(npc, player);
                        }
                    }
                    else
                    {
                        if (player != queue.Peek() )
                        {
                            if (await dialoguePlugin.ShouldGreet(npc, player, message) )
                            {
                                queue.Add(player);

                                await dialoguePlugin.OnBusy(npc, player);
                            }
                        }
                        else
                        {
                            if (await dialoguePlugin.ShouldFarewell(npc, player, message) )
                            {
                                queue.Remove(player);

                                while (queue.Count > 0)
                                {
                                    Player next = queue.Peek();

                                    if (next.Tile == null || next.IsDestroyed || !npc.Tile.Position.IsInRange(next.Tile.Position, 3) )
                                    {
                                        queue.Remove(next);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                if (queue.Count > 0)
                                {
                                    Player next = queue.Peek();

                                    await dialoguePlugin.OnGreet(npc, next);
                                }
                                else
                                {
                                    await dialoguePlugin.OnFarewell(npc, player);
                                }
                            }
                            else
                            {
                                await dialoguePlugin.OnSay(npc, player, message);
                            }
                        }
                    }
                }
            } );

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>(async (context, e) =>
            {
                if (queue.Count == 0)
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

                    if (creatureFocusBehaviour == null || creatureFocusBehaviour.Target != queue.Peek() )
                    {
                        Context.Server.GameObjectComponents.AddComponent(npc, new CreatureFocusBehaviour(queue.Peek() ) );
                    }
                }

                if (queue.Count > 0)
                {
                    var player = queue.Peek();

                    if (player.Tile == null || player.IsDestroyed || !npc.Tile.Position.IsInRange(player.Tile.Position, 3) )
	                {
		                queue.Remove(player);

		                while (queue.Count > 0)
		                {
			                var next = queue.Peek();

			                if (next.Tile == null || next.IsDestroyed || !npc.Tile.Position.IsInRange(next.Tile.Position, 3) )
			                {
				                queue.Remove(next);
			                }
			                else
			                {
				                break;
			                }
		                }

		                if (queue.Count > 0)
		                {
			                var next = queue.Peek();

			                await dialoguePlugin.OnGreet(npc, next);
		                }
                        else
                        {
                            await dialoguePlugin.OnDismiss(npc, player);
                        }
	                }
                }
            } );
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe<PlayerSayEventArgs>(playerSay);

            Context.Server.EventHandlers.Unsubscribe<GlobalTickEventArgs>(globalTick);
        }
    }
}