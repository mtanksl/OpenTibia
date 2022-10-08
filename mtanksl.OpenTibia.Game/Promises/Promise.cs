using System;

namespace OpenTibia.Game.Commands
{
    public class Promise
    {
        public static Promise FromResult(Context context)
        {
            return new Promise(context);
        }

        public static Promise Run(Action< Action<Context> > run)
        {
            return new Promise(run);
        }

        public static Promise Delay(Context context, string key, int executeInMilliseconds)
        {
            return new Promise(resolve =>
            {
                context.Server.QueueForExecution(key, executeInMilliseconds, ctx =>
                {
                    resolve(ctx);
                } );
            } );
        }

        public static Promise Yield(Context context)
        {
            return new Promise(resolve =>
            {
                context.Server.QueueForExecution(ctx =>
                {
                    resolve(ctx);
                } );
            } );
        }

        public static Promise WhenAll(params Promise[] promises)
        {
            return new Promise(resolve =>
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
            return new Promise(resolve =>
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

        private Promise(Context context)
        {
            this.status = PromiseStatus.Fulfilled;

            this.context = context;
        }

        private Promise(Action< Action<Context> > run)
        {
            run(context =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.status = PromiseStatus.Fulfilled;

                    this.context = context;

                    if (this.continueWith != null)
                    {
                        this.continueWith(context);
                    }
                }
            } );
        }

        private Action<Context> continueWith;

        public Promise Then(Action<Context> callback)
        {
            return Promise.Run(resolve =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWith = context =>
                    {
                        callback(context);

                        resolve(context);
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    callback(this.context);

                    resolve(this.context);
                }
            } );
        }

        public Promise Then(Func<Context, Promise> callback)
        {
            return Promise.Run(resolve =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWith = context =>
                    {
                        callback(context).Then(resolve);
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    callback(this.context).Then(resolve);
                }
            } );
        }

        public PromiseResult<TResult> Then<TResult>(Func<Context, PromiseResult<TResult> > callback)
        {
            return PromiseResult<TResult>.Run(resolve =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWith = context =>
                    {
                        callback(context).Then(resolve);
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    callback(this.context).Then(resolve);
                }
            } );
        }
    }
}