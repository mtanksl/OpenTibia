using NLua;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game
{
    public interface ILuaScope
    {
        ILuaScope Parent { get; }

        bool TryGetFunction(string name, out Func<object[], PromiseResult<object[]>> callback);
    }

    public class LuaScripting : ILuaScope, IDisposable
    {
        private Lua env;

        public LuaScripting()
        {
            env = new Lua();

            env.DoString("""

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

                function bridge.load(chunk, env)
                    if not env then
                        env = {}
                        setmetatable(env, {
                            __index = function(table, key)
                                local value = _G[key]
                                if value then
                                    return value
                                end
                                return function(...)
                                    return bridge.call(key, ...)
                                end
                            end
                        } )
                    end
                    local func, errorMessage = load(chunk, nil, "t", env)
                    if func then
                        local success, errorMessage = pcall(func)
                        if success then
                            return true, env
                        end
                        return false, errorMessage
                    end
                    return false, errorMessage
                end

                function print(...)
                    return bridge.call("print", ...)
                end

                """);
        }

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

        public ILuaScope Parent
        {
            get
            {
                return null;
            }
        }

        private Dictionary<string, Func<object[], PromiseResult<object[]>>> callbacks = new Dictionary<string, Func<object[], PromiseResult<object[]>>>();

        public void RegisterFunction(string name, Func<object[], PromiseResult<object[]>> callback)
        {
            callbacks[name] = callback;
        }

        public bool TryGetFunction(string name, out Func<object[], PromiseResult<object[]>> callback)
        {
            return callbacks.TryGetValue(name, out callback);
        }

        public LuaScope LoadChunk(string chunk)
        {
            var loadResult = ( (LuaFunction)this["bridge.load"] ).Call(chunk); // loadResult = success, env = bridge.load(chunk)

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

        public void Dispose()
        {
            env.Dispose();
        }
    }

    public class LuaScope : ILuaScope
    {
        private LuaTable env;

        public LuaScope(ILuaScope parent, LuaTable env)
        {
            this.parent = parent;

            this.env = env;
        }

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

        private ILuaScope parent;

        public ILuaScope Parent
        {
            get
            {
                return parent; 
            }
        }

        public Dictionary<string, Func<object[], PromiseResult<object[]>>> callbacks = new Dictionary<string, Func<object[], PromiseResult<object[]>>>();

        public void RegisterFunction(string name, Func<object[], PromiseResult<object[]>> callback)
        {
            callbacks[name] = callback;
        }

        public bool TryGetFunction(string name, out Func<object[], PromiseResult<object[]>> callback)
        {
            return callbacks.TryGetValue(name, out callback);
        }

        public void LoadChunk(string chunk)
        {
            var loadResult = ( (LuaFunction)this["bridge.load"] ).Call(chunk, env); // loadResult = success, env = bridge.load(chunk)

            var success = (bool)loadResult[0];

            if ( !success)
            {
                var errorMessage = (string)loadResult[1];

                throw new Exception(errorMessage);
            }
        }

        public PromiseResult<object[]> Call(string name, params object[] args)
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

                            ILuaScope current = this;

                            while (true)
                            {
                                if (current == null)
                                {
                                    reject(new NotImplementedException("Function " + name + " is not registered.") );

                                    break;
                                }

                                if (current.TryGetFunction(name, out var callback) )
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
    }
}