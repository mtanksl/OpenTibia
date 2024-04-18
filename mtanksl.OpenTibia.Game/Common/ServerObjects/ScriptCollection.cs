using OpenTibia.Game.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class ScriptCollection : IScriptCollection
    {
        private IServer server;

        public ScriptCollection(IServer server)
        {
            this.server = server;
        }

        public void Start()
        {
#if AOT
            foreach (var script in _AotCompilation.Scripts)
            {
                scripts.Add(script);
            }
#else
            foreach (var type in server.PluginLoader.GetTypes(typeof(Script) ) )
            {
                Script script = (Script)Activator.CreateInstance(type);

                scripts.Add(script);
            }
#endif
            foreach (var script in scripts)
            {
                script.Start();
            }
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
    }
}