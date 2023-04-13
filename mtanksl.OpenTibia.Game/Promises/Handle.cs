using OpenTibia.Game.Commands;
using System;

namespace mtanksl.OpenTibia.Game.Promises
{
    public class Handle
    {
        private Promise promise;

        public Handle()
        {
            promise = new Promise();
        }

        public void TrySetResult()
        {
            promise.TrySetResult();
        }

        public void TrySetException(Exception ex)
        {
            promise.TrySetException(ex);
        }

        public void Wait()
        {
            promise.Wait();
        }
    }
}