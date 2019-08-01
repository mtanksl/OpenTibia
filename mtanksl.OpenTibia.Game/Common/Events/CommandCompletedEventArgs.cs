using OpenTibia.Game;

namespace OpenTibia.Common.Events
{
    public class CommandCompletedEventArgs : GameEventArgs
    {
        public CommandCompletedEventArgs(Server server, Context context) : base(server, context)
        {
            Server = server;

            Context = context;
        }
    }
}