using NLua;
using OpenTibia.Game.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class ScriptCollection : IScriptCollection
    {
        private IServer server;

        public ScriptCollection(IServer server)
        {
            this.server = server;
        }

        ~ScriptCollection()
        {
            Dispose(false);
        }

        private LuaScope script;

        public void Start()
        {
            script = server.LuaScripts.LoadScript(
                server.PathResolver.GetFullPath("data/scripts/config.lua"),
                server.PathResolver.GetFullPath("data/scripts/lib.lua"),
                server.PathResolver.GetFullPath("data/lib.lua"));

            HashSet<Type> types = new HashSet<Type>();

            foreach (LuaTable plugin in ( (LuaTable)script["scripts"] ).Values)
            {
                string fileName = (string)plugin["filename"];

                Type type = server.PluginLoader.GetType(fileName);

                if (types.Add(type) )
                {
                    scripts.Add( (Script)Activator.CreateInstance(type) );
                }
            }

            foreach (var type in server.PluginLoader.GetTypes(typeof(Script) ) )
            {
                if (types.Add(type) )
                {
                    scripts.Add( (Script)Activator.CreateInstance(type) );
                }
            }

            foreach (var script in scripts)
            {
                script.Start();
            }
        }

        public object GetValue(string key)
        {
            return script[key];
        }

        private List<Script> scripts = new List<Script>();

        public T GetScript<T>() where T : Script
        {
            return scripts.OfType<T>().FirstOrDefault();
        }

        public IEnumerable<Script> GetScripts()
        {
            return scripts;
        }

        public void Stop()
        {
            foreach (var script in scripts)
            {
                script.Stop();
            }
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
                    if (script != null)
                    {
                        script.Dispose();
                    }
                }
            }
        }
    }
}