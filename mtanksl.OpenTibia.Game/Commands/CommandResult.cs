using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public abstract class CommandResult<TResult>
    {
        public Context context
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

        public abstract PromiseResult<TResult> Execute();
    }
}