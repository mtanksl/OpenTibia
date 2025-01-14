using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface ILuaScriptCollection : IDisposable
    {
        void Start();

        ILuaScope LoadLib(string libPath, Func<ILuaScope> loadParent);

        ILuaScope LoadLib(params string[] libPaths);

        ILuaScope LoadScript(string scriptPath, ILuaScope parent);

        ILuaScope LoadScript(params string[] scriptPathAndLibPaths);
    }
}