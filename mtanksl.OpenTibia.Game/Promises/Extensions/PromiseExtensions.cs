using OpenTibia.Game.Commands;
using System.Threading.Tasks;

namespace OpenTibia.Game.Promises.Extensions
{
    public static class PromiseExtensions
    {
        public static Task ToTask(this Promise promise)
        {
            TaskCompletionSource tcs = new TaskCompletionSource();

            promise.Catch(ex =>
            {
                tcs.TrySetException(ex);
            } );

            promise.Then( () =>
            {
                tcs.TrySetResult();
            } );

            return tcs.Task;
        }

        public static Task<T> ToTask<T>(this PromiseResult<T> promise)
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

            promise.Catch(ex =>
            {
                tcs.TrySetException(ex);
            } );

            promise.Then(r =>
            {
                tcs.TrySetResult(r);
            } );

            return tcs.Task;
        }
    }
}