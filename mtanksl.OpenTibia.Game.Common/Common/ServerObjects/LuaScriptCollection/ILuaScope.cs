using NLua;
using System;
using System.Reflection;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface ILuaScope : IDisposable
    {
        ILuaScope Parent { get; }

        object this[string key] { get; set; }

        void RegisterFunction(string name, object target, MethodBase method);

        void RegisterCoFunction(string name, Func<ILuaScope, object[], PromiseResult<object[]>> callback);

        bool TryGetCoFunction(string name, out Func<ILuaScope, object[], PromiseResult<object[]>> callback);

        ILuaScope LoadNewChunk(string chunk, string chunkName);

        PromiseResult<object[]> CallFunction(string name, params object[] args);

        PromiseResult<object[]> CallFunction(LuaFunction luaFunction, params object[] args);
    }
}