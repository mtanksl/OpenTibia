using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandler<T> : ICommandHandler where T : Command
    {
        public abstract bool CanHandle(T command, Context context);

        public abstract void Handle(T command, Context context);

        public bool CanHandle(Command command, Context context)
        {
            return CanHandle( (T)command, context);
        }

        public void Handle(Command command, Context context)
        {
            Handle( (T)command, context);
        }
    }
}