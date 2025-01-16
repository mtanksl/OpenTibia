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

                    SkillsMultiplier = new SkillsMultiplier()
                    {
                        MagicLevel = LuaScope.GetDouble(lVocation["skillsmultiplier.magiclevel"] ),
                        
                        Fist = LuaScope.GetDouble(lVocation["skillsmultiplier.fist"] ),

                        Club = LuaScope.GetDouble(lVocation["skillsmultiplier.club"] ),

                        Sword = LuaScope.GetDouble(lVocation["skillsmultiplier.sword"] ),

                        Axe = LuaScope.GetDouble(lVocation["skillsmultiplier.axe"] ),

                        Distance = LuaScope.GetDouble(lVocation["skillsmultiplier.distance"] ),

                        Shield = LuaScope.GetDouble(lVocation["skillsmultiplier.shield"] ),

                        Fish = LuaScope.GetDouble(lVocation["skillsmultiplier.fish"] )
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