using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public abstract class Command
    {
        private Dictionary<string, object> data;

        public Dictionary<string, object> Data
        {
            get
            {
                return data ?? (data = new Dictionary<string, object>() );
            }
        }

        public abstract Promise Execute(Context context);
    }

    public class InlineCommand : Command
    {
        private Func<Context, Promise> execute;

        public InlineCommand(Func<Context, Promise> execute)
        {
            this.execute = execute;
        }

        public override Promise Execute(Context context)
        {
            return execute(context);
        }
    }
}