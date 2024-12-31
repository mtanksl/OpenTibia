using NLua;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class VocationCollection : IVocationCollection
    {
        private IServer server;

        public VocationCollection(IServer server)
        {
            this.server = server;
        }

        ~VocationCollection()
        {
            Dispose(false);
        }

        private ILuaScope script;

        public void Start()
        {
            script = server.LuaScripts.LoadScript(
                server.PathResolver.GetFullPath("data/vocations/config.lua"),
                server.PathResolver.GetFullPath("data/vocations/lib.lua"),
                server.PathResolver.GetFullPath("data/lib.lua") );

            foreach (LuaTable lVocation in ( (LuaTable)script["vocations"] ).Values)
            {
                VocationConfig vocation = new VocationConfig()
                {
                    Id = (byte)(long)lVocation["id"],

                    Name = (string)lVocation["name"],

                    CapacityPerLevel = (int)(long)lVocation["capacityperlevel"],

                    HealthPerLevel = (int)(long)lVocation["healthperlevel"],

                    ManaPerLevel = (int)(long)lVocation["manaperlevel"],

                    Health = (int)(long)lVocation["health"],

                    HealthDelayInSeconds = (int)(long)lVocation["healthdelayinseconds"],

                    Mana = (int)(long)lVocation["mana"],

                    ManaDelayInSeconds = (int)(long)lVocation["manadelayinseconds"]
                };

                vocations.Add(vocation.Id, vocation);
            }
        }

        /// <exception cref="ObjectDisposedException"></exception>
       
        public object GetValue(string key)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(VocationCollection) );
            }

            return script[key];
        }

        private Dictionary<byte, VocationConfig> vocations = new Dictionary<byte, VocationConfig>();

        public VocationConfig GetVocationById(byte id)
        {
            VocationConfig vocation;

            if (vocations.TryGetValue(id, out vocation) )
            {
                return vocation;
            }

            return null;
        }

        public IEnumerable<VocationConfig> GetVocations()
        {
            return vocations.Values;
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