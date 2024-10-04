using NLua;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class LuaScope : ILuaScope
    {
        private Lua lua;

        public LuaScope(IServer server)
        {
            lua = new Lua();

            lua["package.path"] = Path.Combine(server.PathResolver.GetFullPath("data/lualibs"), "?.lua");

            lua["package.cpath"] = Path.Combine(server.PathResolver.GetFullPath("data/clibs"), "?.dll");

            lua.DoString("""

                bridge = {}
                function bridge.call(name, ...)
                	local method = { 
                		name = name,
                		parameters = table.pack(...)
                	};
                	return coroutine.yield(method)
                end
                function bridge.wrap(func)
                	local co = coroutine.create(func)
                	return function(...)
                		local result = { coroutine.resume(co, ...) }
                		if not result[1] then
                			return false, result[2]
                		end
                		local completed = coroutine.status(co) == "dead"
                		return true, completed, select(2, table.unpack(result) )
                	end
                end
                function bridge.load(chunk, chunkName, parent)
                	local env = {}
                	setmetatable(env, {
                		__index = function(table, key)
                			return parent[key]
                		end
                	} )
                	local func, errorMessage = load(chunk, chunkName, "t", env)
                	if func then
                		local success, errorMessage = pcall(func)
                		if success then
                			return true, env
                		end
                		return false, errorMessage
                	end
                	return false, errorMessage
                end

                command = {}
                setmetatable(command, {
                	__index = function(table, key)
                		return function(...)
                			return bridge.call(key, ...)
                		end
                	end
                } )

                debugger = require("mobdebug")
                debugger.coro()

                """);

            this.env = lua.GetTable("_G");

            this.chunkName = "LuaScope.cs";
        }

        public LuaScope(ILuaScope parent, LuaTable env, string chunkName)
        {
            this.parent = parent;

            this.env = env;

            this.chunkName = chunkName;
        }

        ~LuaScope()
        {
            Dispose(false);
        }

        private ILuaScope parent;

        public ILuaScope Parent
        {
            get
            {
                return parent;
            }
        }

        private LuaTable env;

        /// <exception cref="ObjectDisposedException"></exception>

        public object this[string key]
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(LuaScope) );
                }

                return env[key];
            }
            set
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(LuaScope) );
                }

                env[key] = value;
            }
        }

        /// <exception cref="ObjectDisposedException"></exception>

        public void RegisterFunction(string name, object target, MethodBase method)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(LuaScope) );
            }

            if (parent == null)
            {
                lua.RegisterFunction(name, target, method);
            }
            else
            {
                parent.RegisterFunction(name, target, method);
            }
        }

        private Dictionary<string, Func<ILuaScope, object[], PromiseResult<object[]>>> callbacks = new Dictionary<string, Func<ILuaScope, object[], PromiseResult<object[]>>>();

        public void RegisterCoFunction(string name, Func<ILuaScope, object[], PromiseResult<object[]>> callback)
        {
            callbacks[name] = callback;
        }

        public bool TryGetCoFunction(string name, out Func<ILuaScope, object[], PromiseResult<object[]>> callback)
        {
            return callbacks.TryGetValue(name, out callback);
        }

        /// <exception cref="LuaException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>

        public ILuaScope LoadNewChunk(string chunk, string chunkName)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(LuaScope) );
            }

            var loadResult = ( (LuaFunction)env["bridge.load"] ).Call(chunk, chunkName, env);

            var success = (bool)loadResult[0];

            if (success)
            {
                var env = (LuaTable)loadResult[1];

                return new LuaScope(this, env, chunkName);
            }
            else
            {
                var errorMessage = (string)loadResult[1];

                throw new LuaException(chunkName, errorMessage);
            }
        }

        private string chunkName;

        /// <exception cref="ObjectDisposedException"></exception>

        public PromiseResult<object[]> CallFunction(string name, params object[] args)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(LuaScope) );
            }

            return CallFunction( (LuaFunction)env[name], args);
        }

        /// <exception cref="ObjectDisposedException"></exception>

        public PromiseResult<object[]> CallFunction(LuaFunction luaFunction, params object[] args)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(LuaScope) );
            }

            return Promise.Run<object[]>( (resolve, reject) =>
            {
                var wrapResult = (LuaFunction)( (LuaFunction)env["bridge.wrap"] ).Call(luaFunction)[0];

                void Next(object[] args)
                {
                    var pcallResult = wrapResult.Call(args);

                    var success = (bool)pcallResult[0];

                    if (success)
                    {
                        var completed = (bool)pcallResult[1];

                        var result = pcallResult.Skip(2).ToArray();

                        if (completed)
                        {
                            resolve(result);
                        }
                        else
                        {
                            var method = (LuaTable)result[0];

                            var name = (string)method["name"];

                            var parameters = (LuaTable)method["parameters"];

                            ILuaScope current = this;

                            while (true)
                            {
                                if (current == null)
                                {
                                    reject(new LuaException(chunkName, "Function " + name + " is not registered.") );

                                    break;
                                }

                                if (current.TryGetCoFunction(name, out var callback) )
                                {
                                    List<object> values = new List<object>();

                                    for (int i = 1; i <= (int)(long)parameters["n"]; i++)
                                    {
                                        values.Add(parameters[i] );
                                    }

                                    callback(this, values.ToArray() ).Then(Next).Catch(reject);

                                    break;
                                }

                                current = current.Parent;
                            }
                        }
                    }
                    else
                    {
                        var errorMessage = (string)pcallResult[1];

                        reject(new LuaException(chunkName, errorMessage) );
                    }
                }

                Next(args);
            } );
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;

                if (disposing)
                {
                    if (env != null)
                    {
                        env.Dispose();
                    }

                    if (lua != null)
                    {
                        lua.Dispose();
                    }
                }
            }
        }

        public static bool GetBoolean(object value, bool defaultValue = default(bool) )
        {
            if (value != null)
            {
                return (bool)value;
            }

            return defaultValue;
        }

        public static ushort GetUInt16(object value, ushort defaultValue = default(ushort) )
        {
            if (value != null)
            {
                return (ushort)(long)value;
            }

            return defaultValue;
        }

        public static int GetInt32(object value, int defaultValue = default(int) )
        {
            if (value != null)
            {
                return (int)(long)value;
            }

            return defaultValue;
        }

        public static long GetInt64(object value, long defaultValue = default(long) )
        {
            if (value != null)
            {
                return (long)value;
            }

            return defaultValue;
        }

        public static string GetString(object value, string defaultValue = default(string) )
        {
            if (value != null)
            {
                return (string)value;
            }

            return defaultValue;
        }

        public static bool[] GetBooleanArray(object value)
        {
            if (value != null)
            {
                return ( (LuaTable)value).Values.Cast<bool>().ToArray();
            }

            return null;
        }

        public static ushort[] GetUInt16Array(object value)
        {
            if (value != null)
            {
                return ( (LuaTable)value).Values.Cast<long>().Select(v => (ushort)v ).ToArray();
            }

            return null;
        }

        public static int[] GetInt32Array(object value)
        {
            if (value != null)
            {
                return ( (LuaTable)value).Values.Cast<long>().Select(v => (int)v ).ToArray();
            }

            return null;
        }

        public static long[] GetInt64Array(object value)
        {
            if (value != null)
            {
                return ( (LuaTable)value).Values.Cast<long>().ToArray();
            }

            return null;
        }

        public static string[] GetStringArray(object value)
        {
            if (value != null)
            {
                return ( (LuaTable)value).Values.Cast<string>().ToArray();
            }

            return null;
        }
     
        public static List<ushort> GetUInt16List(object value)
        {
            if (value != null)
            {
                var list = new List<ushort>();

                var table = (LuaTable)value;

                foreach (var v in table.Values)
                {
                    list.Add( (ushort)(long)v);
                }

                return list;
            }

            return null;
        }

        public static HashSet<ushort> GetUInt16HashSet(object value)
        {
            if (value != null)
            {
                var hashSet = new HashSet<ushort>();

                var table = (LuaTable)value;

                foreach (var v in table.Values)
                {
                    hashSet.Add( (ushort)(long)v);
                }

                return hashSet;
            }

            return null;
        }

        public static Dictionary<ushort, ushort> GetUInt16IUnt16Dictionary(object value)
        {
            if (value != null)
            {
                var dictionary = new Dictionary<ushort, ushort>();

                var table = (LuaTable)value;

                foreach (var k in table.Keys)
                {
                    var v = table[k];

                    dictionary.Add( (ushort)(long)k, (ushort)(long)v);
                }

                return dictionary;
            }

            return null;
        }
    }
}