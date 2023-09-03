using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenTibia.Game
{
    public class LuaScriptCollection : IDisposable
    {
        private Server server;

        private LuaScope lua;

        public LuaScriptCollection(Server server)
        {
            this.server = server;

            lua = new LuaScope(server);

                lua.RegisterFunction("print", this, GetType().GetMethod(nameof(Print) ) );

                lua.RegisterCoFunction("delay", parameters =>
                {
                    return Promise.Delay(Guid.NewGuid().ToString(), TimeSpan.FromSeconds( (long)parameters[0] ) ).Then( () =>
                    {
                        return Promise.FromResult(Array.Empty<object>() );
                    } );
                } );

                lua.RegisterCoFunction("delaygameobject", parameters =>
                {
                    GameObject gameObject = (GameObject)parameters[0];

                    return Context.Current.Server.GameObjectComponents.AddComponent(gameObject, new DelayBehaviour(TimeSpan.FromSeconds( (long)parameters[0] ) ), false).Promise.Then( () =>
                    {
                        return Promise.FromResult(Array.Empty<object>() );
                    } );
                } );
        }

        ~LuaScriptCollection()
        {
            Dispose(false);
        }

        public void Print(params object[] parameters)
        {
            server.Logger.WriteLine(string.Join("\t", parameters), LogLevel.Debug);
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
                    foreach (var lib in libs)
                    {
                        lib.Value.Dispose();
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