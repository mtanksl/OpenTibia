using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenTibia.Game
{
    public class LuaScriptCollection : IDisposable
    {
        private LuaScope lua;

        public LuaScriptCollection(Server server)
        {
            lua = new LuaScope(server);

            lua.RegisterFunction("print", this, GetType().GetMethod(nameof(Print) ) );

            lua.RegisterCoFunction("type", parameters =>
            {
                return Promise.FromResult(new object[] { parameters[0].GetType().Name } );
            } );

            lua.RegisterCoFunction("delay", parameters =>
            {                   
                string key = Guid.NewGuid().ToString();

                Promise promise = Promise.Delay(key, TimeSpan.FromSeconds( (long)parameters[0] ) );

                if (parameters.Length == 2)
                {
                    _ = promise.Then( () =>
                    {
                        ( (LuaFunction)parameters[1] ).Call();
                    } ); 

                    return Promise.FromResult(new object[] { key } );
                }

                return promise.Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );                
            } );

            lua.RegisterCoFunction("delaygameobject", parameters =>
            {
                DelayBehaviour delayBehaviour = Context.Current.Server.GameObjectComponents.AddComponent( (GameObject)parameters[0], new DelayBehaviour(TimeSpan.FromSeconds( (long)parameters[1] ) ), false);

                if (parameters.Length == 3)
                {
                    _ = delayBehaviour.Promise.Then( () =>
                    {
                        ( (LuaFunction)parameters[2] ).Call();
                    } );

                    return Promise.FromResult(new object[] { delayBehaviour.Key } );
                }

                return delayBehaviour.Promise.Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("canceldelay", parameters =>
            {
                bool canceled = Context.Current.Server.CancelQueueForExecution( (string)parameters[0] );

                return Promise.FromResult(new object[] { canceled } );
            } );

            //TODO: Creature add condition, creature attack area, creature attack creature, creature remove condition

            lua.RegisterCoFunction("creaturewalk", parameters =>
            {
                return Context.Current.AddCommand(new CreatureWalkCommand( (Creature)parameters[0], ToTile(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("creatureupdatedirection", parameters =>
            {
                return Context.Current.AddCommand(new CreatureUpdateDirectionCommand( (Creature)parameters[0], (Direction)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("creatureupdatehealth", parameters =>
            {
                return Context.Current.AddCommand(new CreatureUpdateHealthCommand( (Creature)parameters[0], (int)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("creatureupdateinvisible", parameters =>
            {
                return Context.Current.AddCommand(new CreatureUpdateInvisibleCommand( (Creature)parameters[0], (bool)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("creatureupdatelight", parameters =>
            {
                return Context.Current.AddCommand(new CreatureUpdateLightCommand( (Creature)parameters[0], ToLight(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("creatureupdateoutfit", parameters =>
            {
                return Context.Current.AddCommand(new CreatureUpdateOutfitCommand( (Creature)parameters[0], ToOutfit(parameters[1] ), ToOutfit(parameters[2] ) ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("creatureupdatepartyicon", parameters =>
            {
                return Context.Current.AddCommand(new CreatureUpdatePartyIconCommand( (Creature)parameters[0], (PartyIcon)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("creatureupdateskullicon", parameters =>
            {
                return Context.Current.AddCommand(new CreatureUpdateSkullIconCommand( (Creature)parameters[0], (SkullIcon)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("creatureupdatespeed", parameters =>
            {
                return Context.Current.AddCommand(new CreatureUpdateSpeedCommand( (Creature)parameters[0], (ushort)(long)parameters[1], (ushort)(long)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("showanimatedtext", parameters =>
            {
                return Context.Current.AddCommand(new ShowAnimatedTextCommand( (Position)parameters[0], (AnimatedTextColor)(long)parameters[1], (string)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("showmagiceffect", parameters =>
            {
                return Context.Current.AddCommand(new ShowMagicEffectCommand( (Position)parameters[0], (MagicEffectType)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("showprojectile", parameters =>
            {
                return Context.Current.AddCommand(new ShowProjectileCommand( (Position)parameters[0], (Position)parameters[1], (ProjectileType)(long)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("showtext", parameters =>
            {
                return Context.Current.AddCommand(new ShowTextCommand( (Creature)parameters[0], (TalkType)(long)parameters[1], (string)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("showwindowtext", parameters =>
            {
                Context.Current.AddPacket( ( (Player)parameters[0] ).Client.Connection, new ShowWindowTextOutgoingPacket( (TextColor)(long)parameters[1], (string)parameters[2] ) );

                return Promise.FromResult(Array.Empty<object>() );
            } );

            lua.RegisterCoFunction("fluiditemupdatefluidtype", parameters =>
            {
                return Context.Current.AddCommand(new FluidItemUpdateFluidTypeCommand( (FluidItem)parameters[0], (FluidType)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("itemdestroy", parameters =>
            {
                return Context.Current.AddCommand(new ItemDestroyCommand( (Item)parameters[0] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("itemtransform", parameters =>
            {
                return Context.Current.AddCommand(new ItemTransformCommand( (Item)parameters[0], (ushort)(long)parameters[1], (byte)(long)parameters[2] ) ).Then( (item) =>
                {
                    return Promise.FromResult(new object[] { item } );
                } );
            } );

            lua.RegisterCoFunction("monsterdestroy", parameters =>
            {
                return Context.Current.AddCommand(new MonsterDestroyCommand( (Monster)parameters[0] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );
            
            lua.RegisterCoFunction("monstersay", parameters =>
            {
                return Context.Current.AddCommand(new MonsterSayCommand( (Monster)parameters[0], (string)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("npcdestroy", parameters =>
            {
                return Context.Current.AddCommand(new NpcDestroyCommand( (Npc)parameters[0] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("npcsay", parameters =>
            {
                return Context.Current.AddCommand(new NpcSayCommand( (Npc)parameters[0], (string)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            //TODO: Player update axe, club, distance, fish, fist, magic level, shield and sword skills

            lua.RegisterCoFunction("playeraddmoney", parameters =>
            {
                return Context.Current.AddCommand(new PlayerAddMoneyCommand((Player)parameters[0], (int)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("playerremovemoney", parameters =>
            {
                return Context.Current.AddCommand(new PlayerRemoveMoneyCommand((Player)parameters[0], (int)(long)parameters[1] ) ).Then( (success) =>
                {
                    return Promise.FromResult(new object[] { success } );
                } );                
            } );

            lua.RegisterCoFunction("playercountmoney", parameters =>
            {
                return Context.Current.AddCommand(new PlayerCountMoneyCommand((Player)parameters[0] ) ).Then( (price) =>
                {
                    return Promise.FromResult(new object[] { price } );
                } );                  
            } );

            lua.RegisterCoFunction("playeradditem", parameters =>
            {
                return Context.Current.AddCommand(new PlayerAddItemCommand( (Player)parameters[0], (ushort)(long)parameters[1], (byte)(long)parameters[2], (int)(long)parameters[3] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("playerremoveitem", parameters =>
            {
                return Context.Current.AddCommand(new PlayerRemoveItemCommand( (Player)parameters[0], (ushort)(long)parameters[1], (byte)(long)parameters[2], (int)(long)parameters[3] ) ).Then( (success) =>
                {
                    return Promise.FromResult(new object[] { success } );
                } );                
            } );

            lua.RegisterCoFunction("playercountitem", parameters =>
            {
                return Context.Current.AddCommand(new PlayerCountItemCommand( (Player)parameters[0], (ushort)(long)parameters[1], (byte)(long)parameters[2] ) ).Then( (count) =>
                {
                    return Promise.FromResult(new object[] { count } );
                } );                
            } );

            lua.RegisterCoFunction("playerdestroy", parameters =>
            {
                return Context.Current.AddCommand(new PlayerDestroyCommand( (Player)parameters[0] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("playerupdatecapacity", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateCapacityCommand( (Player)parameters[0], (int)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("playerupdateexperience", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateExperienceCommand( (Player)parameters[0], (uint)(long)parameters[1], (ushort)(long)parameters[2], (byte)(long)parameters[3] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("playerupdatemana", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateManaCommand( (Player)parameters[0], (int)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("playerupdatesoul", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateSoulCommand( (Player)parameters[0], (int)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("playerupdatestamina", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateStaminaCommand( (Player)parameters[0], (int)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            lua.RegisterCoFunction("playergetstorage", parameters =>
            {
                Player player = (Player)parameters[0];

                int value;

                if (player.Client.Storages.TryGetValue( (int)(long)parameters[1], out value) )
                {
                    return Promise.FromResult(new object[] { true, value } );
                }

                return Promise.FromResult(new object[] { false } );
            } );

            lua.RegisterCoFunction("playersetstorage", parameters =>
            {
                Player player = (Player)parameters[0];

                player.Client.Storages.SetValue( (int)(long)parameters[1], (int)(long)parameters[2] );

                return Promise.FromResult(Array.Empty<object>() );
            } );

            lua.RegisterCoFunction("splashitemupdatefluidtype", parameters =>
            {
                return Context.Current.AddCommand(new SplashItemUpdateFluidTypeCommand( (SplashItem)parameters[0], (FluidType)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );
                                    
            lua.RegisterCoFunction("stackableitemupdatecount", parameters =>
            {
                return Context.Current.AddCommand(new StackableItemUpdateCountCommand( (StackableItem)parameters[0], (byte)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResult(Array.Empty<object>() );
                } );
            } );

            this.server = server;
        }

        ~LuaScriptCollection()
        {
            Dispose(false);
        }

        private Server server;

        public void Print(params object[] parameters)
        {
            server.Logger.WriteLine(string.Join("\t", parameters), LogLevel.Debug);
        }

        private Light ToLight(object parameter)
        {
            if (parameter is Light)
            {
                return (Light)parameter;
            }
            
            if (parameter is LuaTable)
            {
                LuaTable table = (LuaTable)parameter;

                return new Light( (byte)(long)table["level"], (byte)(long)table["color"] );
            }
         
            throw new ArgumentException();
        }

        private Outfit ToOutfit(object parameter)
        {
            if (parameter is Light)
            {
                return (Outfit)parameter;
            }
            
            if (parameter is LuaTable)
            {
                LuaTable table = (LuaTable)parameter;

                if (table["itemid"] != null)
                {
                    return new Outfit( (ushort)(long)table["itemid"] );
                }

                return new Outfit( (ushort)(long)table["id"], (byte)(long)table["head"], (byte)(long)table["body"], (byte)(long)table["legs"], (byte)(long)table["feet"], (Addon)(long)table["addon"] );
            }
         
            throw new ArgumentException();
        }

        private Position ToPosition(object parameter)
        {
            if (parameter is Position)
            {
                return (Position)parameter;
            }
            
            if (parameter is LuaTable)
            {
                LuaTable table = (LuaTable)parameter;

                return new Position( (int)table["x"], (int)table["y"], (int)table["z"] );
            }
         
            throw new ArgumentException();
        }

        private Tile ToTile(object parameter)
        {
            if (parameter is Tile)
            {
                return (Tile)parameter;
            }
            
            if (parameter is LuaTable)
            {
                return Context.Current.Server.Map.GetTile(ToPosition(parameter) );
            }
         
            throw new ArgumentException();
        }

        private Dictionary<string, string> chunks = new Dictionary<string, string>();

        public string GetChunk(string path)
        {
            string chunk;

            if ( !chunks.TryGetValue(path, out chunk) )
            {
                chunk = File.ReadAllText(path);

                chunks.Add(path, chunk);
            }

            return chunk;
        }

        private Dictionary<string, LuaScope> libs = new Dictionary<string, LuaScope>();

        public LuaScope Create(string libPath1, string libPath2, string scriptPath)
        {
            LuaScope lib2;

            if ( !libs.TryGetValue(libPath2, out lib2) )
            {
                LuaScope lib1;

                if ( !libs.TryGetValue(libPath1, out lib1) )
                {
                    lib1 = lua.LoadNewChunk(GetChunk(libPath1), libPath1);

                    libs.Add(libPath1, lib1);
                }

                lib2 = lib1.LoadNewChunk(GetChunk(libPath2), libPath2);
                                 
                libs.Add(libPath2, lib2);
            }

            LuaScope script = lib2.LoadNewChunk(GetChunk(scriptPath), scriptPath);

            return script;
        }

        public LuaScope Create(string libPath, string scriptPath)
        {
            LuaScope lib;

            if ( !libs.TryGetValue(libPath, out lib) )
            {
                lib = lua.LoadNewChunk(GetChunk(libPath), libPath);

                libs.Add(libPath, lib);
            }

            LuaScope script = lib.LoadNewChunk(GetChunk(scriptPath), scriptPath);

            return script;
        }

        public LuaScope Create(string scriptPath)
        {
            LuaScope script = lua.LoadNewChunk(GetChunk(scriptPath), scriptPath);

            return script;
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;

                if (disposing)
                {
                    if (libs != null)
                    {
                        foreach (var lib in libs)
                        {
                            lib.Value.Dispose();
                        }
                    }

                    if (lua != null)
                    {
                        lua.Dispose();
                    }
                }
            }
        }
    }
}