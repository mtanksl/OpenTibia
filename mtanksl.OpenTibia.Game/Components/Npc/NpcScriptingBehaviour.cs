using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class NpcScriptingBehaviour : Behaviour
    {
        private string script;

        private IWalkStrategy walkStrategy;

        public NpcScriptingBehaviour(string script, IWalkStrategy walkStrategy)
        {
            this.script = script;

            this.walkStrategy = walkStrategy;
        }

        private Guid playerSay;

        private Guid globalTick;

        public override void Start()
        {
            Npc npc = (Npc)GameObject;

            QueueHashSet<Player> targets = new QueueHashSet<Player>();

            LuaScope lua = Context.Server.LuaScripts.Load("data/npcs/lib/npc.lua", "data/npcs/scripts/" + script);

                lua.RegisterFunction("npcsay", parameters =>
                {
                    return Context.AddCommand(new NpcSayCommand(npc, (string)parameters[0] ) ).Then( () =>
                    {
                        return Promise.FromResult(Array.Empty<object>() );
                    } );
                } );

            playerSay = Context.Server.EventHandlers.Subscribe<PlayerSayEventArgs>(async (context, e) =>
            {
                Player player = e.Player;

                if (npc.Tile.Position.IsInRange(player.Tile.Position, 3) )
                {               
                    string message = e.Message;

                    if (targets.Count == 0)
                    {
                        var proceed = await lua.Call("shouldgreet", npc, player, message);

                        if ( (bool)proceed[0] )
                        {
                            targets.Add(player);

                            await lua.Call("greet", npc, player);
                        }
                    }
                    else
                    {
                        if (player != targets.Peek() )
                        {                             
                            var proceed = await lua.Call("shouldgreet", npc, player, message);

                            if ( (bool)proceed[0] )
                            {
                                targets.Add(player);

                                await lua.Call("busy", npc, player);
                            }
                        }
                        else
                        {
                            var proceed = await lua.Call("shouldfarewell", npc, player, message);

                            if ( (bool)proceed[0] )
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

                                    await lua.Call("greet", npc, next);
                                }
                                else
                                {
                                    await lua.Call("farewell", npc, player);
                                }
                            }
                            else
                            {
                                await lua.Call("say", npc, player, message);
                            }
                        }
                    }
                }
            } );

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>(async (context, e) =>
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

                            await lua.Call("greet", npc, next);
		                }
                        else
                        {
                            await lua.Call("dismiss", npc, player);
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