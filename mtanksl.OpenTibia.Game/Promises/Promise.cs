using System;

namespace OpenTibia.Game.Commands
{
    public class Promise
    {
        public static Promise Pending()
        {
            return new Promise();
        }

        public static Promise Completed()
        {
            return Promise.Run( (resolve, reject) =>
            {
                resolve();
            } );
        }

        public static Promise Run(Action<Action, Action<Exception> > run)
        {
            return new Promise(run);
        }

        public static PromiseResult<TResult> FromResult<TResult>(TResult result)
        {
            return Promise.Run<TResult>( (resolve, reject) =>
            {
                resolve(result);
            } );
        }

        public static PromiseResult<TResult> Run<TResult>(Action<Action<TResult>, Action<Exception> > run)
        {
            return new PromiseResult<TResult>(run);
        }

        public static Promise Yield(Server server)
        {
            return server.QueueForExecution( () =>
            {
                return Promise.Completed();
            } );
        }

        public static Promise Delay(Server server, string key, int executeInMilliseconds)
        {
            return server.QueueForExecution(key, executeInMilliseconds, () =>
            {
                return Promise.Completed();
            } );
        }

        public static Promise WhenAll(params Promise[] promises)
        {
            return Promise.Run( (resolve, reject) =>
            {
                int index = 0;

                for (int i = 0; i < promises.Length; i++)
                {
                    promises[i].Then( () =>
                    {
                        if (++index == promises.Length)
                        {
                            resolve();
                        }
                    } );
                }
            } );
        }

        public static Promise WhenAny(params Promise[] promises)
        {
            return Promise.Run( (resolve, reject) =>
            {
                int index = 0;

                for (int i = 0; i < promises.Length; i++)
                {
                    promises[i].Then( () =>
                    {
                        if (++index == 1)
                        {
                            resolve();
                        }
                    } );
                }
            } );
        }

        private PromiseStatus status;

        private Exception exception;

        private Action continueWithFulfilled;

        private Action<Exception> continueWithRejected;

        public Promise()
        {
            this.status = PromiseStatus.Pending;
        }

        public Promise(Action<Action, Action<Exception> > run)
        {
            run(Resolve, Reject);
        }

        private void Resolve()
        {
            TrySetResult();
        }

        private void Reject(Exception exception)
        {
            TrySetException(exception);
        }

        public bool TrySetResult()
        {
            if (this.status == PromiseStatus.Pending)
            {
                this.status = PromiseStatus.Fulfilled;

                if (this.continueWithFulfilled != null)
                {
                    this.continueWithFulfilled();
                }

                return true;
            }

            return false;
        }

        public bool TrySetException(Exception exception)
        {
            if (this.status == PromiseStatus.Pending)
            {
                this.status = PromiseStatus.Rejected;

                this.exception = exception;

                if (this.continueWithRejected != null)
                {
                    this.continueWithRejected(this.exception);
                }

                return true;
            }

            return false;
        }

        public Promise Then(Action onFullfilled, Action<Exception> onRejected = null)
        {
            return Promise.Run( (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = () =>
                    {
                        onFullfilled();

                        resolve();
                    };

                    this.continueWithRejected = (e) =>
                    {
                        if (onRejected != null)
                        {
                            onRejected(e);
                        }

                        reject(e);
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    onFullfilled();

                    resolve();
                }
                else if (this.status == PromiseStatus.Rejected)
                {
                    if (onRejected != null)
                    {
                        onRejected(this.exception);
                    }

                    reject(this.exception);
                }
            } );
        }

        public Promise Then(Func<Promise> onFullfilled, Func<Exception, Promise> onRejected = null)
        {
            return Promise.Run( (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = () =>
                    {
                        onFullfilled().Then(resolve, reject);
                    };

                    this.continueWithRejected = (e) =>
                    {
                        if (onRejected != null)
                        {
                            onRejected(e).Then(resolve, reject);
                        }
                        else
                        {
                            reject(e);
                        }                       
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    onFullfilled().Then(resolve, reject);
                }
                else if (this.status == PromiseStatus.Rejected)
                {
                    if (onRejected != null)
                    {
                        onRejected(this.exception).Then(resolve, reject);
                    }
                    else
                    {
                        reject(this.exception);
                    }                    
                }
            } );
        }

        public PromiseResult<TResult> Then<TResult>(Func<PromiseResult<TResult> > onFullfilled, Func<Exception, PromiseResult<TResult> > onRejected = null)
        {
            return Promise.Run<TResult>( (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = () =>
                    {
                        onFullfilled().Then(resolve, reject);
                    };

                    this.continueWithRejected = (e) =>
                    {
                        if (onRejected != null)
                        {
                            onRejected(e).Then(resolve, reject);
                        }
                        else
                        {
                            reject(e);
                        }                       
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    onFullfilled().Then(resolve, reject);
                }
                else if (this.status == PromiseStatus.Rejected)
                {
                    if (onRejected != null)
                    {
                        onRejected(this.exception).Then(resolve, reject);
                    }
                    else
                    {
                        reject(this.exception);
                    }
                }
            } );
        }
    }
}