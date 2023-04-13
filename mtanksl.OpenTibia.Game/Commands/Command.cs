using System.Collections.Generic;
using System.Diagnostics;

namespace OpenTibia.Game.Commands
{
    public abstract class Command
    {
        public static Promise Sequence(params Command[] commands)
        {
            int i = -1;

            [DebuggerStepThrough] Promise Next()
            {
                i++;

                if (i < commands.Length)
                {
                    return Context.Current.AddCommand(commands[i] ).Then(Next);
                }

                return Promise.Completed;
            }

            return Next();
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