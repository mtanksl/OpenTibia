using System;

namespace OpenTibia.Game.Commands
{
    public class PromiseResult<TResult>
    {
        private PromiseStatus status;

        private Context context;

        private TResult result;

        public PromiseResult(Action< Action<Context, TResult> > run)
        {
            run( (context, result) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.status = PromiseStatus.Fulfilled;

                    this.context = context;

                    this.result = result;

                    if (this.continueWith != null)
                    {
                        this.continueWith(context, result);
                    }
                }
            } );
        }

        private Action<Context, TResult> continueWith;

        public PromiseResult<TResult> Then(Action<Context, TResult> callback)
        {
            return new PromiseResult<TResult>(resolve =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWith = (context, result) =>
                    {
                        callback(context, result);

                        resolve(context, result);
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    callback(this.context, this.result);

                    resolve(this.context, this.result);
                }
            } );
        }

        public PromiseResult<TResult> Then(Func<Context, TResult, PromiseResult<TResult> > callback)
        {
            return new PromiseResult<TResult>(resolve =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWith = (context, result) =>
                    {
                        var promise = callback(context, result);

                        if (promise != null)
                        {
                            promise.Then(resolve);
                        }
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    var promise = callback(this.context, this.result);

                    if (promise != null)
                    {
                        promise.Then(resolve);
                    }
                }
            } );
        }

        public Promise Then(Func<Context, TResult, Promise> callback)
        {
            return new Promise(resolve =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWith = (context, result) =>
                    {
                        var promise = callback(context, result);

                        if (promise != null)
                        {
                            promise.Then(resolve);
                        }
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    var promise = callback(this.context, this.result);

                    if (promise != null)
                    {
                        promise.Then(resolve);
                    }
                }
            } );
        }        
    }
}