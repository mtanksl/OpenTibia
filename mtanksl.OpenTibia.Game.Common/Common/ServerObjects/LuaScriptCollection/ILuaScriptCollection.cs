using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface ILuaScriptCollection : IDisposable
    {
        string GetFullPath(string relativePath);

        string GetChunk(string path);

        bool TryGetLib(string libPath, out ILuaScope lib);

        ILuaScope LoadLib(string libPath, Func<ILuaScope> loadParent);

        ILuaScope LoadLib(params string[] libPaths);

        ILuaScope LoadScript(string scriptPath, ILuaScope parent);

        ILuaScope LoadScript(params string[] scriptPathAndLibPaths);
    }
}