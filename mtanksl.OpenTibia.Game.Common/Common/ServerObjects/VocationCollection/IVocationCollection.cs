using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IVocationCollection : IDisposable
    {
        void Start();

        object GetValue(string key);

        VocationConfig GetVocationById(byte id);

        IEnumerable<VocationConfig> GetVocations();
    }
}