using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class Values : IValues
    {
        private IServer server;

        public Values(IServer server)
        {
            this.server = server;
        }

        ~Values()
        {
            Dispose(false);
        }
               
        private ILuaScope script;

        public void Start()
        {
            script = server.LuaScripts.LoadScript(
                server.PathResolver.GetFullPath("data/values/config.lua"),
                server.PathResolver.GetFullPath("data/values/lib.lua"),
                server.PathResolver.GetFullPath("data/lib.lua") );
        }

        /// <exception cref="ObjectDisposedException"></exception>

        public object GetValue(string key)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(Config) );
            }

            return script[key];
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
                    if (script != null)
                    {
                        script.Dispose();
                    }
                }
            }
        }
    }
}