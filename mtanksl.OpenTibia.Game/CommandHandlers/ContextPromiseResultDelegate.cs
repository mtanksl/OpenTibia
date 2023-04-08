using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public delegate PromiseResult<TResult> ContextPromiseResultDelegate<TResult>(Context context);
}