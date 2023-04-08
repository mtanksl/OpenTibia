using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public abstract class CommandResult<TResult>
    {
        private Dictionary<string, object> data;

        public Dictionary<string, object> Data
        {
            get
            {
                return data ?? (data = new Dictionary<string, object>() );
            }
        }

        public abstract PromiseResult<TResult> Execute(Context context);
    }
}