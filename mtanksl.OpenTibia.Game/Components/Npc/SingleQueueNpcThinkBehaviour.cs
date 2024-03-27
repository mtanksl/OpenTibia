using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class SingleQueueNpcThinkBehaviour : Behaviour
    {
        private DialoguePlugin dialoguePlugin;

        private IWalkStrategy walkStrategy;

        public SingleQueueNpcThinkBehaviour(DialoguePlugin dialoguePlugin, IWalkStrategy walkStrategy)
        {
            this.dialoguePlugin = dialoguePlugin;

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

        private DateTime lastSay;

        private Guid playerSay;

        private Guid playerSayToNpc;

        private Guid globalTick;

        public override void Start()
        {
            Npc npc = (Npc)GameObject;

            playerSay = Context.Server.EventHandlers.Subscribe<PlayerSayEventArgs>( (context, e) => Say(e.Player, e.Message) );
            
            playerSayToNpc = Context.Server.EventHandlers.Subscribe<PlayerSayToNpcEventArgs>( (context, e) => Say(e.Player, e.Message) );

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
                    Player target = queue.Peek();

                    if (target.Tile == null || target.IsDestroyed || !npc.Tile.Position.IsInRange(target.Tile.Position, 3) || (DateTime.UtcNow - lastSay).TotalMinutes >= 1)
                    {
                        await Disappear(target);
                    }
                    else
                    {
                        CreatureWalkBehaviour creatureWalkBehaviour = Context.Server.GameObjectComponents.GetComponent<CreatureWalkBehaviour>(npc);

                        if (creatureWalkBehaviour != null)
                        {
                            Context.Server.GameObjectComponents.RemoveComponent(npc, creatureWalkBehaviour);
                        }

                        CreatureFocusBehaviour creatureFocusBehaviour = Context.Server.GameObjectComponents.GetComponent<CreatureFocusBehaviour>(npc);

                        if (creatureFocusBehaviour == null || creatureFocusBehaviour.Target != target)
                        {
                            Context.Server.GameObjectComponents.AddComponent(npc, new CreatureFocusBehaviour(target) );
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
            Context.Server.EventHandlers.Unsubscribe<PlayerSayEventArgs>(playerSay);

            Context.Server.EventHandlers.Unsubscribe<PlayerSayToNpcEventArgs>(playerSayToNpc);
            
            Context.Server.EventHandlers.Unsubscribe<GlobalTickEventArgs>(globalTick);
        }
    }
}