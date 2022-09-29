using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandler : ICommandHandler
    {
        public Action<Context> Continuation { get; set; }

        public abstract bool CanHandle(Context context, Command command);

        public virtual void Handle(Context context, Command command)
        {
            if (Continuation != null)
            {
                Continuation(context);
            }
        }
    }

    public abstract class CommandHandler<T> : CommandHandler where T : Command
    {
        public override bool CanHandle(Context context, Command command)
        {
            return CanHandle(context, (T)command);
        }

        public abstract bool CanHandle(Context context, T command);

        public override void Handle(Context context, Command command)
        {
            Handle(context, (T)command);
        }

        public virtual void Handle(Context context, T command)
        {
            base.Handle(context, command);
        }
    }

    public class InlineCommandHandler<T> : CommandHandler<T> where T : Command
    {
        private Func<Context, T, bool> canHandle;

        private Action<Context, T, Action<Context, T> > handle;

        public InlineCommandHandler(Func<Context, T, bool> canHandle, Action<Context, T, Action<Context, T> > handle)
        {
            this.canHandle = canHandle;

            this.handle = handle;
        }

        public override bool CanHandle(Context context, T command)
        {
            return canHandle(context, command);
        }

        public override void Handle(Context context, T command)
        {
            handle(context, command, base.Handle);
        }
    }
}