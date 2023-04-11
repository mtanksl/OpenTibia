using System;

namespace OpenTibia.Game.Commands
{
    public class PromiseResult<TResult>
    {
        private PromiseStatus status;

        private TResult result;

        private Exception exception;

        private Action<TResult> continueWithFulfilled;

        private Action<Exception> continueWithRejected;

        public PromiseResult()
        {
            this.status = PromiseStatus.Pending;
        }

        public PromiseResult(Action<Action<TResult>, Action<Exception> > run)
        {
            run(Resolve, Reject);
        }

        private void Resolve(TResult result)
        {
            TrySetResult(result);
        }

        private void Reject(Exception exception)
        {
            TrySetException(exception);
        }

        public bool TrySetResult(TResult result)
        {
            if (this.status == PromiseStatus.Pending)
            {
                this.status = PromiseStatus.Fulfilled;

                this.result = result;

                if (this.continueWithFulfilled != null)
                {
                    this.continueWithFulfilled(this.result);
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

        public PromiseResult<TResult> Then(Action<TResult> onFullfilled, Action<Exception> onRejected = null)
        {
            return Promise.Run<TResult>( (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = (r) =>
                    {
                        onFullfilled(r);

                        resolve(r);
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
                    onFullfilled(this.result);

                    resolve(this.result);
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

        public PromiseResult<TResult> Then(Func<TResult, PromiseResult<TResult> > onFullfilled, Func<Exception, PromiseResult<TResult> > onRejected = null)
        {
            return Promise.Run<TResult>( (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = (r) =>
                    {
                        onFullfilled(r).Then(resolve, reject);
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
                    onFullfilled(this.result).Then(resolve, reject);
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

        public PromiseResult<TResult2> Then<TResult2>(Func<TResult, PromiseResult<TResult2> > onFullfilled, Func<Exception, PromiseResult<TResult2> > onRejected = null)
        {
            return Promise.Run<TResult2>( (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = (r) =>
                    {
                        onFullfilled(r).Then(resolve, reject);
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
                    onFullfilled(this.result).Then(resolve, reject);
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

        public Promise Then(Func<TResult, Promise> onFullfilled, Func<Exception, Promise> onRejected = null)
        {
            return Promise.Run( (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = (r) =>
                    {
                        onFullfilled(r).Then(resolve, reject);
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
                    onFullfilled(this.result).Then(resolve, reject);
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
        
        public static implicit operator Promise(PromiseResult<TResult> promise)
        {
            return promise.Then( (r) => 
            {
                return Promise.Completed();
            } );
        }
    }
}