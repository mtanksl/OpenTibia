using OpenTibia.Game.Commands;
using System.Threading.Tasks;

namespace OpenTibia.Game.Promises.Extensions
{
    public static class TaskExtensions
    {
        public static Promise ToPromise(this Task task)
        {
            Promise promise = new Promise();

            task.ContinueWith(t =>
            {
                if (t.IsCanceled)
                {
                    promise.TrySetException(new PromiseCanceledException() );
                }
                else if (t.IsFaulted)
                {
                    promise.TrySetException(t.Exception);
                }
                else
                {
                    promise.TrySetResult();
                }
            } );

            return promise;        
        }

        public static PromiseResult<T> ToPromise<T>(this Task<T> task)
        {
            PromiseResult<T> promise = new PromiseResult<T>();

            task.ContinueWith(t =>
            {
                if (t.IsCanceled)
                {
                    promise.TrySetException(new PromiseCanceledException() );
                }
                else if (t.IsFaulted)
                {
                    promise.TrySetException(t.Exception);
                }
                else
                {
                    promise.TrySetResult(t.Result);
                }
            } );

            return promise;
        }
    }
}