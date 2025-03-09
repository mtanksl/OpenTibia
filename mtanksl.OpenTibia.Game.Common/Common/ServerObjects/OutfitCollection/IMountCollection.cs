using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IMountCollection : IDisposable
    {
        void Start();

        object GetValue(string key);

        MountConfig GetMountById(ushort id);

        IEnumerable<MountConfig> GetMounts();
    }
}