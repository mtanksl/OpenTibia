using OpenTibia.Game;
using System;

namespace OpenTibia.Common.Events
{
    public class CommandCompletedEventArgs : EventArgs
    {
        public CommandCompletedEventArgs(Context context)
        {
            Context = context;
        }

        public Context Context { get; set; }
    }
}