using OpenTibia.Game.Scripts;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IScriptCollection : IDisposable
    {
        void Start();

        object GetValue(string key);

        T GetScript<T>() where T : Script;

        IEnumerable<Script> GetScripts();

        void Stop();
    }
}