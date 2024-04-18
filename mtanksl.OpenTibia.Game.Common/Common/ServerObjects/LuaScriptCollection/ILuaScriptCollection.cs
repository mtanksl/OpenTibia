using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface ILuaScriptCollection : IDisposable
    {
        string GetFullPath(string relativePath);

        string GetChunk(string path);

        bool TryGetLib(string libPath, out LuaScope lib);

        LuaScope LoadLib(string libPath, Func<LuaScope> loadParent);

        LuaScope LoadLib(params string[] libPaths);

        LuaScope LoadScript(string scriptPath, LuaScope parent);

        LuaScope LoadScript(params string[] scriptPathAndLibPaths);
    }
}