using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace OpenTibia.Game.Commands
{
    [DebuggerStepThrough]
    [AsyncMethodBuilder(typeof(PromiseResultMethodBuilder<>))]

    public class PromiseResult<TResult> : INotifyCompletion
    {
        private readonly object sync = new object();

        private PromiseStatus status;

        private TResult result;

        private Exception exception;

        private List<Action<TResult>> continueWithFulfilled;

        private List<Action<Exception>> continueWithRejected;

        public PromiseResult()
        {
            status = PromiseStatus.Pending;
        }

        public PromiseResult(Action<Action<TResult>, Action<Exception> > run)
        {
            try
            {
                run( (r) => { TrySetResult(r); }, (ex) => { TrySetException(ex); } );
            }
            catch (Exception ex)
            {
                TrySetException(ex);
            }
        }

        public bool TrySetResult(TResult r)
        {
            lock (sync)
            {
                if (status == PromiseStatus.Pending)
                {
                    status = PromiseStatus.Fulfilled;

                    result = r;

                    if (continueWithFulfilled != null)
                    {
                        foreach (var item in continueWithFulfilled)
                        {
                            item(result);
                        }                    
                    }

                    return true;
                }

                return false;
            }
        }

        public bool TrySetException(Exception ex)
        {
            lock (sync)
            {
                if (status == PromiseStatus.Pending)
                {
                    status = PromiseStatus.Rejected;

                    exception = ex;

                    if (continueWithRejected != null)
                    {
                        foreach (var item in continueWithRejected)
                        {
                            item(exception);
                        }
                    }

                    return true;
                }

                return false;
            }
        }

        private void AddContinueWithFulfilled(Action<TResult> next)
        {
            if (continueWithFulfilled == null)
            {
                continueWithFulfilled = new List<Action<TResult>>();
            }

            continueWithFulfilled.Add(next);
        }

        private void AddContinueWithRejected(Action<Exception> next)
        {
            if (continueWithRejected == null)
            {
                continueWithRejected = new List<Action<Exception>>();
            }

            continueWithRejected.Add(next);
        }

        public TResult Result
        {
            get
            {
                lock (sync)
                {
                    if (status == PromiseStatus.Pending)
                    {
                        TResult result = default(TResult);

                        AddContinueWithFulfilled( (r) =>
                        {
                            result = r;

                            Monitor.Pulse(sync);
                        } );

                        Exception exception = null;

                        AddContinueWithRejected( (ex) =>
                        {
                            exception = ex;

                            Monitor.Pulse(sync);
                        } );

                        Monitor.Wait(sync);

                        if (exception != null)
                        {
                            throw exception;
                        }

                        return result;                        
                    }
                    else
                    {
                        if (exception != null)
                        {
                            throw exception;
                        }

                        return result;
                    }
                }
            }
        }

        public Promise Catch(Action<Exception> onRejected)
        {
            return Promise.Run( (resolve, reject) =>
            {
                lock (sync)
                {
                    if (status == PromiseStatus.Pending)
                    {
                        AddContinueWithFulfilled( (r) =>
                        {
                            resolve();
                        } );

                        AddContinueWithRejected( (ex) =>
                        {
                            onRejected(ex);

                            reject(ex);
                        } );
                    }
                    else if (status == PromiseStatus.Fulfilled)
                    {
                        resolve();
                    }
                    else if (status == PromiseStatus.Rejected)
                    {
                        onRejected(exception);

                        reject(exception);
                    }
                }
            } );
        }   

        public PromiseResult<TResult> Then(Action<TResult> onFullfilled)
        {
            return Promise.Run<TResult>( (resolve, reject) =>
            {
                lock (sync)
                {
                    if (status == PromiseStatus.Pending)
                    {
                        AddContinueWithFulfilled( (r) =>
                        {
                            onFullfilled(r);

                            resolve(r);
                        } );

                        AddContinueWithRejected( (ex) =>
                        {
                            reject(ex);
                        } );
                    }
                    else if (status == PromiseStatus.Fulfilled)
                    {
                        onFullfilled(result);

                        resolve(result);
                    }
                    else if (status == PromiseStatus.Rejected)
                    {
                        reject(exception);
                    }
                }
            } );
        }

        public Promise Then(Func<TResult, Promise> onFullfilled)
        {
            return Promise.Run( (resolve, reject) =>
            {
                lock (sync)
                {
                    if (status == PromiseStatus.Pending)
                    {
                        AddContinueWithFulfilled( (r) =>
                        {
                            onFullfilled(r).Then(resolve).Catch(reject);
                        } );

                        AddContinueWithRejected( (ex) =>
                        {
                            reject(ex);
                        } );
                    }
                    else if (status == PromiseStatus.Fulfilled)
                    {
                        onFullfilled(result).Then(resolve).Catch(reject);
                    }
                    else if (status == PromiseStatus.Rejected)
                    {
                        reject(exception);
                    }
                }
            } );
        }    

        public PromiseResult<TResult2> Then<TResult2>(Func<TResult, TResult2> onFullfilled)
        {
            return Promise.Run<TResult2>( (resolve, reject) =>
            {
                lock (sync)
                {
                    if (status == PromiseStatus.Pending)
                    {
                        AddContinueWithFulfilled( (r) =>
                        {
                            resolve(onFullfilled(r) );
                        } );

                        AddContinueWithRejected( (ex) =>
                        {
                            reject(ex);
                        } );
                    }
                    else if (status == PromiseStatus.Fulfilled)
                    {
                        resolve(onFullfilled(result) );
                    }
                    else if (status == PromiseStatus.Rejected)
                    {
                        reject(exception);
                    }
                }
            } );
        }

        public PromiseResult<TResult2> Then<TResult2>(Func<TResult, PromiseResult<TResult2> > onFullfilled)
        {
            return Promise.Run<TResult2>( (resolve, reject) =>
            {
                lock (sync)
                {
                    if (status == PromiseStatus.Pending)
                    {
                        AddContinueWithFulfilled( (r) =>
                        {
                            onFullfilled(r).Then(resolve).Catch(reject);
                        } );

                        AddContinueWithRejected( (ex) =>
                        {
                            reject(ex);
                        } );
                    }
                    else if (status == PromiseStatus.Fulfilled)
                    {
                        onFullfilled(result).Then(resolve).Catch(reject);
                    }
                    else if (status == PromiseStatus.Rejected)
                    {
                        reject(exception);
                    }
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

        public PromiseResult<TResult> GetAwaiter()
        {
            return this;
        }

        public bool IsCompleted
        {
            get
            {
                lock (sync)
                {
                    return status != PromiseStatus.Pending;
                }
            }
        }

        public void OnCompleted(Action next)
        {
            // Pending

            lock (sync)
            {
                AddContinueWithFulfilled( (r) =>
                {
                    next();
                } );     
            
                AddContinueWithRejected( (ex) =>
                {
                    next();
                } );
            }
        }

        public TResult GetResult()
        {
            // Fulfilled or Rejected

            lock(sync)
            {
                if (exception != null)
                {
                    throw exception;
                }

                return result;
            }
        }
    }

    [DebuggerStepThrough]

    public class PromiseResultMethodBuilder<T>
    {
        public static PromiseResultMethodBuilder<T> Create()
        {
            return new PromiseResultMethodBuilder<T>();
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            //
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion
                                                                                                                    where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion
                                                                                                                          where TStateMachine : IAsyncStateMachine
        {
            awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
        }

        private PromiseResult<T> promise;

        public PromiseResult<T> Task
        {
            get
            {
                return promise ?? (promise = new PromiseResult<T>() );
            }
        }

        public void SetResult(T result)
        {
            Task.TrySetResult(result);
        }

        public void SetException(Exception ex)
        {
            Task.TrySetException(ex);
        }
    }
}