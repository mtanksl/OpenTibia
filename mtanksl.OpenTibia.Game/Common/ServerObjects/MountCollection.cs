using NLua;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class MountCollection : IMountCollection
    {
        private IServer server;

        public MountCollection(IServer server)
        {
            this.server = server;
        }

        ~MountCollection()
        {
            Dispose(false);
        }

        private ILuaScope script;

        public void Start()
        {
            script = server.LuaScripts.LoadScript(
                server.PathResolver.GetFullPath("data/mounts/config.lua") ,
                server.PathResolver.GetFullPath("data/mounts/lib.lua"),
                server.PathResolver.GetFullPath("data/lib.lua") );

            foreach (LuaTable lMount in ( (LuaTable)script["mounts"] ).Values)
            {
                MountConfig mount = new MountConfig()
                {
                    Id = LuaScope.GetUInt16(lMount["id"] ),

                    Name = LuaScope.GetString(lMount["name"] ),

                    Speed = LuaScope.GetInt32(lMount["speed"] ),

                    Premium = LuaScope.GetBoolean(lMount["premium"] ),

                    AvailableAtOnce = LuaScope.GetBoolean(lMount["availableatonce"] ),

                    MinClientVersion = LuaScope.GetInt32(lMount["minclientversion"] )
                };

                mounts.Add(mount.Id, mount);
            }
        }

        /// <exception cref="ObjectDisposedException"></exception>
      
        public object GetValue(string key)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(MountCollection) );
            }

            return script[key];
        }

        private Dictionary<ushort, MountConfig> mounts = new Dictionary<ushort, MountConfig>();

        public MountConfig GetMountById(ushort id)
        {
            MountConfig mount;

            if (mounts.TryGetValue(id, out mount) )
            {
                return mount;
            }

            return null;
        }

        public IEnumerable<MountConfig> GetMounts()
        {
            return mounts.Values;
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