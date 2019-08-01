using OpenTibia.Game;
using System;

namespace OpenTibia.Common.Events
{
    public abstract class GameEventArgs : EventArgs
    {
        protected GameEventArgs(Server server, Context context)
        {
            Server = server;

            Context = context;
        }

        public Server Server { get; set; }

        public Context Context { get; set; }
    }
}