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
                    Id = LuaScope.GetByte(lVocation["id"] ),

                    Name = LuaScope.GetString(lVocation["name"] ),

                    Description = LuaScope.GetString(lVocation["description"] ),

                    CapacityPerLevel = LuaScope.GetInt32(lVocation["capacityperlevel"] ),

                    HealthPerLevel = LuaScope.GetInt32(lVocation["healthperlevel"] ),

                    ManaPerLevel = LuaScope.GetInt32(lVocation["manaperlevel"] ),

                    RegenerateHealth = LuaScope.GetInt32(lVocation["regeneratehealth"] ),

                    RegenerateHealthInSeconds = LuaScope.GetInt32(lVocation["regeneratehealthinseconds"] ),

                    RegenerateMana = LuaScope.GetInt32(lVocation["regeneratemana"] ),

                    RegenerateManaInSeconds = LuaScope.GetInt32(lVocation["regeneratemanainseconds"] ),

                    RegenerateSoul = LuaScope.GetInt32(lVocation["regeneratesoul"] ),

                    RegenerateSoulInSeconds = LuaScope.GetInt32(lVocation["regeneratesoulinseconds"] ),

                    SoulMax = LuaScope.GetInt32(lVocation["soulmax"] ),

                    VocationConstants = new VocationConstantsConfig()
                    {
                        MagicLevel = LuaScope.GetDouble(lVocation["vocationconstants.magiclevel"] ),
                        
                        Fist = LuaScope.GetDouble(lVocation["vocationconstants.fist"] ),

                        Club = LuaScope.GetDouble(lVocation["vocationconstants.club"] ),

                        Sword = LuaScope.GetDouble(lVocation["vocationconstants.sword"] ),

                        Axe = LuaScope.GetDouble(lVocation["vocationconstants.axe"] ),

                        Distance = LuaScope.GetDouble(lVocation["vocationconstants.distance"] ),

                        Shield = LuaScope.GetDouble(lVocation["vocationconstants.shield"] ),

                        Fish = LuaScope.GetDouble(lVocation["vocationconstants.fish"] )
                    }
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