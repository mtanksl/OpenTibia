using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandlerResult<TResult> : ICommandHandlerResult<TResult>
    {
        public Action<Context, TResult> ContinueWith { get; set; }

        public abstract bool CanHandle(Context context, CommandResult<TResult> command);

        public abstract void Handle(Context context, CommandResult<TResult> command);

        protected virtual void OnComplete(Context context, TResult result)
        {
            if (ContinueWith != null)
            {
                ContinueWith(context, result);
            }
        }
    }

    public abstract class CommandHandlerResult<T, TResult> : CommandHandlerResult<TResult> where T : CommandResult<TResult>
    {
        public override bool CanHandle(Context context, CommandResult<TResult> command)
        {
            return CanHandle(context, (T)command);
        }

        public abstract bool CanHandle(Context context, T command);

        public override void Handle(Context context, CommandResult<TResult> command)
        {
            Handle(context, (T)command);
        }

        public abstract void Handle(Context context, T command);
    }

    public class InlineCommandHandlerResult<T, TResult> : CommandHandlerResult<T, TResult> where T : CommandResult<TResult>
    {
        private Func<Context, T, bool> canHandle;

        private Action<Context, T, Action<Context, TResult> > handle;

        public InlineCommandHandlerResult(Func<Context, T, bool> canHandle, Action<Context, T, Action<Context, TResult> > handle)
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