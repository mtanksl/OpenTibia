using System;

namespace OpenTibia.Game.Commands
{
    public class PromiseResult<TResult>
    {
        public static PromiseResult<TResult> Break()
        {
            return new PromiseResult<TResult>();
        }

        public static PromiseResult<TResult> FromResult(Context context, TResult result)
        {
            return new PromiseResult<TResult>(context, result);
        }

        public static PromiseResult<TResult> Run(Action<Action<Context, TResult> > run)
        {
            return new PromiseResult<TResult>(run);
        }

        private PromiseStatus status;

        private Context context;

        private TResult result;

        private PromiseResult()
        {
            this.status = PromiseStatus.Pending;
        }

        private PromiseResult(Context context, TResult result)
        {
            this.status = PromiseStatus.Fulfilled;

            this.context = context;

            this.result = result;
        }

        private PromiseResult(Action< Action<Context, TResult> > run)
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
                
        /// <exception cref="InvalidOperationException"></exception>

        public TResult Result
        {
            get
            {
                if (status != PromiseStatus.Fulfilled)
                {
                    throw new InvalidOperationException("Promise is not fulfilled.");
                }

                return result;
            }
        }

        private Action<Context, TResult> continueWith;

        public PromiseResult<TResult> Then(Action<Context, TResult> callback)
        {
            return PromiseResult<TResult>.Run(resolve =>
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
            return PromiseResult<TResult>.Run(resolve =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWith = (context, result) =>
                    {
                        callback(context, result).Then(resolve);
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    callback(this.context, this.result).Then(resolve);
                }
            } );
        }

        public Promise Then(Func<Context, TResult, Promise> callback)
        {
            return Promise.Run(resolve =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWith = (context, result) =>
                    {
                        callback(context, result).Then(resolve);
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    callback(this.context, this.result).Then(resolve);
                }
            } );
        }     
        
        public static implicit operator Promise(PromiseResult<TResult> promise)
        {
            return promise.Then( (ctx, result) => 
            {
                return Promise.FromResult(ctx);
            } );
        }
    }
}