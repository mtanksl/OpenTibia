using OpenTibia.Game.Commands;
using System;

namespace mtanksl.OpenTibia.Game.Promises
{
    public struct Handle
    {
        private Promise promise;

        public Handle()
        {
            promise = new Promise();
        }

        public bool TrySetResult()
        {
            return promise.TrySetResult();
        }

        public bool TrySetException(Exception ex)
        {
            return promise.TrySetException(ex);
        }

        public void Wait()
        {
            promise.Wait();
        }
    }
}