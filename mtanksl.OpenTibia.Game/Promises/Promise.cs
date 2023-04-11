using System;

namespace OpenTibia.Game.Commands
{
    public class Promise
    {
        public static Promise Pending()
        {
            return new Promise();
        }

        public static Promise Completed(Context context)
        {
            return Promise.Run( (resolve, reject) =>
            {
                resolve(context);
            } );
        }

        public static Promise Run(Action<Action<Context>, Action<Context, Exception> > run)
        {
            return new Promise(run);
        }

        public static PromiseResult<TResult> FromResult<TResult>(Context context, TResult result)
        {
            return Promise.Run<TResult>( (resolve, reject) =>
            {
                resolve(context, result);
            } );
        }

        public static PromiseResult<TResult> Run<TResult>(Action<Action<Context, TResult>, Action<Context, Exception> > run)
        {
            return new PromiseResult<TResult>(run);
        }

        public static Promise Yield(Server server)
        {
            return server.QueueForExecution(ctx =>
            {
                return Promise.Completed(ctx);
            } );
        }

        public static Promise Delay(Server server, string key, int executeInMilliseconds)
        {
            return server.QueueForExecution(key, executeInMilliseconds, ctx =>
            {
                return Promise.Completed(ctx);
            } );
        }

        public static Promise WhenAll(params Promise[] promises)
        {
            return Promise.Run( (resolve, reject) =>
            {
                int index = 0;

                for (int i = 0; i < promises.Length; i++)
                {
                    promises[i].Then(ctx =>
                    {
                        if (++index == promises.Length)
                        {
                            resolve(ctx);
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
                    promises[i].Then(ctx =>
                    {
                        if (++index == 1)
                        {
                            resolve(ctx);
                        }
                    } );
                }
            } );
        }

        private PromiseStatus status;

        private Context context;

        private Exception exception;

        private Action<Context> continueWithFulfilled;

        private Action<Context, Exception> continueWithRejected;

        public Promise()
        {
            this.status = PromiseStatus.Pending;
        }

        public Promise(Action<Action<Context>, Action<Context, Exception> > run)
        {
            Action<Context> resolve = (c) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.status = PromiseStatus.Fulfilled;

                    this.context = c;

                    if (this.continueWithFulfilled != null)
                    {
                        this.continueWithFulfilled(this.context);
                    }
                }
            };

            Action<Context, Exception> reject = (c, e) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.status = PromiseStatus.Rejected;

                    this.context = c;

                    this.exception = e;

                    if (this.continueWithRejected != null)
                    {
                        this.continueWithRejected(this.context, this.exception);
                    }
                }
            };

            run(resolve, reject);
        }

        public bool TrySetResult()
        {
            if (this.status == PromiseStatus.Pending)
            {
                this.status = PromiseStatus.Fulfilled;

                if (this.continueWithFulfilled != null)
                {
                    this.continueWithFulfilled(this.context);
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
                    this.continueWithRejected(this.context, this.exception);
                }

                return true;
            }

            return false;
        }

        public Promise Then(Action<Context> onFullfilled, Action<Context, Exception> onRejected = null)
        {
            return Promise.Run( (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = (c) =>
                    {
                        onFullfilled(c);

                        resolve(c);
                    };

                    this.continueWithRejected = (c, e) =>
                    {
                        onRejected(c, e);

                        reject(c, e);
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    onFullfilled(this.context);

                    resolve(this.context);
                }
                else if (this.status == PromiseStatus.Rejected)
                {
                    onRejected(this.context, this.exception);

                    reject(this.context, this.exception);
                }
            } );
        }

        public Promise Then(Func<Context, Promise> onFullfilled, Func<Context, Exception, Promise> onRejected = null)
        {
            return Promise.Run( (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = (c) =>
                    {
                        onFullfilled(c).Then(resolve, reject);
                    };

                    this.continueWithRejected = (c, e) =>
                    {
                        onRejected(c, e).Then(resolve, reject);
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    onFullfilled(this.context).Then(resolve, reject);
                }
                else if (this.status == PromiseStatus.Rejected)
                {
                    onRejected(this.context, this.exception).Then(resolve, reject);
                }
            } );
        }

        public PromiseResult<TResult> Then<TResult>(Func<Context, PromiseResult<TResult> > onFullfilled, Func<Context, Exception, PromiseResult<TResult> > onRejected = null)
        {
            return Promise.Run<TResult>( (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = (c) =>
                    {
                        onFullfilled(c).Then(resolve, reject);
                    };

                    this.continueWithRejected = (c, e) =>
                    {
                        onRejected(c, e).Then(resolve, reject);
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    onFullfilled(this.context).Then(resolve, reject);
                }
                else if (this.status == PromiseStatus.Rejected)
                {
                    onRejected(this.context, this.exception).Then(resolve, reject);
                }
            } );
        }
    }
}