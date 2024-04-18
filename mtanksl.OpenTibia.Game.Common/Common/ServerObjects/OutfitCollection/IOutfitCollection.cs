using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IOutfitCollection : IDisposable
    {
        void Start();

        object GetValue(string key);

        OutfitConfig GetOutfitById(ushort id);

        IEnumerable<OutfitConfig> GetOutfits();
    }
}