using OpenTibia.Game.Scripts;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IScriptCollection
    {
        void Start();

        T GetScript<T>() where T : Script;

        IEnumerable<Script> GetScripts();

        void Stop();
    }
}