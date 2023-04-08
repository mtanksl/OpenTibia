using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandler<T> : CommandHandler where T : Command
    {
        public override Promise Handle(Context context, ContextPromiseDelegate next, Command command)
        {
            return Handle(context, next, (T)command);
        }

        public abstract Promise Handle(Context context, ContextPromiseDelegate next, T command);
    }
}