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
                	env.mt = {
                		__index = function(table, key)
                			return parent[key]
                		end
                	}
                	setmetatable(env, env.mt)
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

                command.mt = {
                	__index = function(table, key)
                		return function(...)
                			return bridge.call(key, ...)
                		end
                	end
                }

                setmetatable(command, command.mt)

                debugger = require("mobdebug")

                debugger.coro()

                """);

            this.env = lua.GetTable("_G");

            this.chunkName = "LuaScope.cs";
        }

        public LuaScope(LuaScope parent, LuaTable env, string chunkName)
        {
            this.parent = parent;

            this.env = env;

            this.chunkName = chunkName;
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

        public PromiseResult<object[]> CallFunction(string name, params object[] args)
        {
            return Promise.Run<object[]>( (resolve, reject) =>
            {
                var wrapResult = (LuaFunction)( (LuaFunction)env["bridge.wrap"] ).Call( (LuaFunction)env[name] )[0];

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

                            LuaScope current = this;

                            while (true)
                            {
                                if (current == null)
                                {
                                    reject(new LuaException(chunkName, "Function " + name + " is not registered.") );

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
    }
}