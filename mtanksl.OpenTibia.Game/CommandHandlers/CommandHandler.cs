using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandler : ICommandHandler
    {
        public Action<Context> ContinueWith { get; set; }

        public abstract bool CanHandle(Context context, Command command);

        public abstract void Handle(Context context, Command command);

        protected virtual void OnComplete(Context context)
        {
            if (ContinueWith != null)
            {
                ContinueWith(context);
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

        public abstract void Handle(Context context, T command);
    }

    public class InlineCommandHandler<T> : CommandHandler<T> where T : Command
    {
        private Func<Context, T, bool> canHandle;

        private Action<Context, T, Action<Context> > handle;

        public InlineCommandHandler(Func<Context, T, bool> canHandle, Action<Context, T, Action<Context> > handle)
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
            handle(context, command, OnComplete);
        }
    }
}