using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public abstract class Command
    {
        public static Promise WhenAll(params Command[] commands)
        {
            if (commands == null || commands.Length == 0)
            {
                return Promise.Completed;
            }

            Context context = Context.Current;

            List<Promise> promises = new List<Promise>();

            foreach (Command command in commands) 
            { 
                promises.Add(context.AddCommand(command) );
            }

            return Promise.WhenAll(promises.ToArray() );
        }

        public static Promise WhenAny(params Command[] commands)
        {
            if (commands == null || commands.Length == 0)
            {
                return Promise.Completed;
            }

            Context context = Context.Current;

            List<Promise> promises = new List<Promise>();

            foreach (Command command in commands) 
            { 
                promises.Add(context.AddCommand(command) );
            }

            return Promise.WhenAny(promises.ToArray() );
        }

        public Context Context
        {
            get
            {
                return Context.Current;
            }
        }

        private Dictionary<string, object> data;

        public Dictionary<string, object> Data
        {
            get
            {
                return data ?? (data = new Dictionary<string, object>() );
            }
        }

        public abstract Promise Execute();       
    }
}