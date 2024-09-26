using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IValues : IDisposable
    {
        void Start();

        object GetValue(string key);
    }
}