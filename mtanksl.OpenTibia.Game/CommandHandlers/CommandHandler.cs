using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandler<T> : ICommandHandler where T : Command
    {
        public abstract bool CanHandle(T command, Context context);

        public abstract Command Handle(T command, Context context);

        public bool CanHandle(Command command, Context context)
        {
            return CanHandle( (T)command, context);
        }

        public Command Handle(Command command, Context context)
        {
            return Handle( (T)command, context);
        }
    }
}