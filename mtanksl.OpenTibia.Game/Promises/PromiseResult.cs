using System;
using System.Diagnostics;

namespace OpenTibia.Game.Commands
{
    [DebuggerStepThrough]
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
            try
            {
                run(Resolve, Reject);
            }
            catch (Exception ex)
            {
                Reject(ex);
            }
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

        public Promise Catch(Action<Exception> onRejected)
        {
            return Promise.Run( [DebuggerStepThrough] (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = (r) =>
                    {
                        resolve();
                    };

                    this.continueWithRejected = (e) =>
                    {
                        onRejected(e);

                        reject(e);
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    resolve();
                }
                else if (this.status == PromiseStatus.Rejected)
                {
                    onRejected(this.exception);

                    reject(this.exception);
                }
            } );
        }   

        public PromiseResult<TResult> Then(Action<TResult> onFullfilled)
        {
            return Promise.Run<TResult>( [DebuggerStepThrough] (resolve, reject) =>
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
                    reject(this.exception);
                }
            } );
        }

        public PromiseResult<TResult> Then(Func<TResult, PromiseResult<TResult> > onFullfilled)
        {
            return Promise.Run<TResult>( [DebuggerStepThrough] (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = (r) =>
                    {
                        onFullfilled(r).Then(resolve).Catch(reject);
                    };

                    this.continueWithRejected = (e) =>
                    {
                        reject(e);                     
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    onFullfilled(this.result).Then(resolve).Catch(reject);
                }
                else if (this.status == PromiseStatus.Rejected)
                {
                    reject(this.exception);                
                }
            } );
        }

        public PromiseResult<TResult2> Then<TResult2>(Func<TResult, PromiseResult<TResult2> > onFullfilled)
        {
            return Promise.Run<TResult2>( [DebuggerStepThrough] (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = (r) =>
                    {
                        onFullfilled(r).Then(resolve).Catch(reject);
                    };

                    this.continueWithRejected = (e) =>
                    {
                        reject(e);
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    onFullfilled(this.result).Then(resolve).Catch(reject);
                }
                else if (this.status == PromiseStatus.Rejected)
                {
                    reject(this.exception);
                }
            } );
        }

        public Promise Then(Func<TResult, Promise> onFullfilled)
        {
            return Promise.Run( [DebuggerStepThrough] (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = (r) =>
                    {
                        onFullfilled(r).Then(resolve).Catch(reject);
                    };

                    this.continueWithRejected = (e) =>
                    {
                        reject(e);
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    onFullfilled(this.result).Then(resolve).Catch(reject);
                }
                else if (this.status == PromiseStatus.Rejected)
                {
                    reject(this.exception);
                }
            } );
        }     
        
        public static implicit operator Promise(PromiseResult<TResult> promise)
        {
            return promise.Then( (r) => 
            {
                return Promise.Completed;
            } );
        }          
    }
}