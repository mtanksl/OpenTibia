using NLua;
using OpenTibia.Common.Structures;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class OutfitCollection : IDisposable
    {
        private Server server;

        public OutfitCollection(Server server)
        {
            this.server = server;
        }

        private LuaScope script;

        public void Start()
        {
            script = server.LuaScripts.Create(server.PathResolver.GetFullPath("data/lib.lua"), server.PathResolver.GetFullPath("data/outfits/lib.lua"), server.PathResolver.GetFullPath("data/outfits/config.lua") );

            foreach (LuaTable lOutfit in ( (LuaTable)script["outfits"] ).Values)
            {
                OutfitConfig outfit = new OutfitConfig()
                {
                    Id = (ushort)(long)lOutfit["id"],

                    Name = (string)lOutfit["name"],

                    Gender = (Gender)(int)(long)lOutfit["gender"]
                };

                outfits.Add(outfit.Id, outfit);
            }
        }

        public object GetValue(string key)
        {
            return script[key];
        }

        private Dictionary<ushort, OutfitConfig> outfits = new Dictionary<ushort, OutfitConfig>();

        public OutfitConfig GetOutfitById(ushort id)
        {
            OutfitConfig outfit;

            if (outfits.TryGetValue(id, out outfit) )
            {
                return outfit;
            }

            return null;
        }

        public IEnumerable<OutfitConfig> GetOutfits()
        {
            return outfits.Values;
        }

        public void Dispose()
        {
            script.Dispose();
        }
    }
}