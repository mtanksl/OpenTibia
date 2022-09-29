using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandHandler
    {
        bool CanHandle(Context context, Command command);

        void Handle(Context context, Command command);

        Action<Context> Continuation { get; set; }
    }
}