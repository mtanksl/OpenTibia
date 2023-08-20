using OpenTibia.Game.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenTibia.Game
{
    public class ScriptsCollection
    {
        public ScriptsCollection()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(Script).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract) )
            {
                Script script = (Script)Activator.CreateInstance(type);

                scripts.Add(script);
            }
        }

        private List<Script> scripts = new List<Script>();

        public void Start()
        {
            foreach (var script in scripts)
            {
                script.Start();
            }
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