using NLua;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OpenTibia.Game
{
    public class LuaScope : IDisposable
    {
        private Lua lua;

        public LuaScope(Server server)
        {
            lua = new Lua();

            lua["package.path"] = Path.Combine(server.PathResolver.GetFullPath("data/lualibs"), "?.lua");

            lua["package.cpath"] = Path.Combine(server.PathResolver.GetFullPath("data/clibs"), "?.dll");

            lua.DoString("""

	            debugger = require("mobdebug")

	            debugger.coro()

	            """);

            lua.DoString("""

                bridge = {}

                function bridge.call(name, ...)
                    local method = { 
                        name = name,
                        parameters = { ... } 
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
                            local value = parent[key]
                            if value then
                                return value
                            end
                            return function(...)
                                return bridge.call(key, ...)
                            end
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

                """);

            this.parent = null;

            this.env = (LuaTable)lua["_G"];
        }

        public LuaScope(LuaScope parent, LuaTable env)
        {
            this.parent = parent;

            this.env = env;
        }

        ~LuaScope()
        {
            Dispose(false);
        }

        private LuaScope parent;

        public LuaScope Parent
        {
            get
            {
                return parent;
            }
        }

        private LuaTable env;

        public object this[string key]
        {
            get
            {
                return env[key];
            }
            set
            {
                env[key] = value;
            }
        }

        public void RegisterFunction(string name, object target, MethodBase method)
        {
            if (parent == null)
            {
                lua.RegisterFunction(name, target, method);
            }
            else
            {
                parent.RegisterFunction(name, target, method);
            }
        }

        private Dictionary<string, Func<object[], PromiseResult<object[]>>> callbacks = new Dictionary<string, Func<object[], PromiseResult<object[]>>>();

        public void RegisterCoFunction(string name, Func<object[], PromiseResult<object[]>> callback)
        {
            callbacks[name] = callback;
        }

        public bool TryGetCoFunction(string name, out Func<object[], PromiseResult<object[]>> callback)
        {
            return callbacks.TryGetValue(name, out callback);
        }        

        public LuaScope LoadNewChunk(string chunk, string chunkName)
        {
            var loadResult = ( (LuaFunction)this["bridge.load"] ).Call(chunk, chunkName, env); // loadResult = success, env = bridge.load(chunk, chunkName, env)

            var success = (bool)loadResult[0];

            if (success)
            {
                var env = (LuaTable)loadResult[1];

                return new LuaScope(this, env);
            }
            else
            {
                var errorMessage = (string)loadResult[1];

                throw new Exception(errorMessage);
            }
        }

        public PromiseResult<object[]> CallFunction(string name, params object[] args)
        {
            return Promise.Run<object[]>( (resolve, reject) =>
            {
                var wrapResult = (LuaFunction)( (LuaFunction)this["bridge.wrap"] ).Call( (LuaFunction)this[name] )[0]; // wrapResult = bridge.wrap(func)

                void Next(object[] args)
                {
                    var pcallResult = wrapResult.Call(args); // pcallResult = success, completed, result = wrapResult(arg0, arg1, arg2)

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
                            var method = (LuaTable)result[0]; // method = { name = ..., parameters = { ... } }

                            var name = (string)method["name"];

                            LuaScope current = this;

                            while (true)
                            {
                                if (current == null)
                                {
                                    reject(new NotImplementedException("Function " + name + " is not registered.") );

                                    break;
                                }

                                if (current.TryGetCoFunction(name, out var callback) )
                                {
                                    var parameters = ( (LuaTable)method["parameters"] ).Values.Cast<object>().ToArray();

                                    callback(parameters).Then(Next).Catch(reject);

                                    break;
                                }
                              
                                current = current.Parent;
                            }
                        }
                    }
                    else
                    {
                        var errorMessage = (string)pcallResult[1];

                        reject(new Exception(errorMessage) );
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
    }
}