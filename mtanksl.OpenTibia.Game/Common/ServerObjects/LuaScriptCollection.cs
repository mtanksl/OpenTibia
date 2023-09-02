using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenTibia.Game
{
    public class LuaScriptCollection : IDisposable
    {
        private LuaScope luaScope;

        public LuaScriptCollection(Server server)
        {
            luaScope = new LuaScope(server);

                luaScope.RegisterFunction("print", parameters =>
                {
                    server.Logger.WriteLine(string.Join("\t", parameters), LogLevel.Debug);

                    return Promise.FromResult(Array.Empty<object>() );
                } );

                luaScope.RegisterFunction("delay", parameters =>
                {
                    return Promise.Delay(Guid.NewGuid().ToString(), TimeSpan.FromSeconds( (long)parameters[0] ) ).Then( () =>
                    {
                        return Promise.FromResult(Array.Empty<object>() );
                    } );
                } );
        }

        ~LuaScriptCollection()
        {
            Dispose(false);
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

        public LuaScope Load(params string[] paths)
        {
            LuaScope lua = luaScope.LoadNewChunk(GetChunk(paths[0] ), paths[0] );

            for (int i = 1; i < paths.Length; i++)
            {
                lua.LoadChunk(GetChunk(paths[i] ), paths[i] );
            }
            
            return lua;
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
                    if (luaScope != null)
                    {
                        luaScope.Dispose();
                    }
                }
            }
        }
    }
}