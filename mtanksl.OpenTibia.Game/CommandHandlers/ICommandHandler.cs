using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandHandler
    {
        Action<Context> ContinueWith { get; set; }

        bool CanHandle(Context context, Command command);

        void Handle(Context context, Command command);
    }
}