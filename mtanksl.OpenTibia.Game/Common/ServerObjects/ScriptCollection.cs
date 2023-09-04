using OpenTibia.Game.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenTibia.Game
{
    public class ScriptCollection
    {
        public void Start()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(Script).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract) )
            {
                Script script = (Script)Activator.CreateInstance(type);

                scripts.Add(script);
            }

            foreach (var script in scripts)
            {
                script.Start();
            }
        }

        private List<Script> scripts = new List<Script>();

        public void Stop()
        {
            foreach (var script in scripts)
            {
                script.Stop();
            }
        }
    }
}