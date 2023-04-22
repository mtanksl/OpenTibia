using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace OpenTibia.Game.Commands
{
    [DebuggerStepThrough]
    [AsyncMethodBuilder(typeof(PromiseMethodBuilder))]

    public class Promise : INotifyCompletion
    {
        private static Promise completed = Promise.Run( (resolve, reject) =>
        {
            resolve();
        } );

        public static Promise Completed
        {
            get
            {
                return completed;
            }
        }

        private static Promise broken = Promise.Run( (resolve, reject) =>
        {
            Exception ex = new PromiseCanceledException();

            reject(ex);
        } );

        public static Promise Break
        {
            get
            {
                return broken;
            }
        }

        public static PromiseResult<TResult> FromResult<TResult>(TResult result)
        {
            return Promise.Run<TResult>( (resolve, reject) =>
            {
                resolve(result);
            } );
        }

        public static PromiseResult<TResult> Run<TResult>(Func<TResult> run)
        {
            return Promise.Run<TResult>( (resolve, reject) =>
            {
                resolve(run() );
            } );
        }

        public static PromiseResult<TResult> Run<TResult>(Func<PromiseResult<TResult>> run)
        {
            return Promise.Run<TResult>( (resolve, reject) =>
            {
                run().Then(resolve).Catch(reject);
            } );
        }

        public static PromiseResult<TResult> Run<TResult>(Action<Action<TResult>, Action<Exception> > run)
        {
            return new PromiseResult<TResult>(run);
        }
        
        public static Promise FromException(Exception exception)
        {
            return Promise.Run( (resolve, reject) =>
            {
                reject(exception);
            } );
        }

        public static Promise Run(Action run)
        {
            return Promise.Run( (resolve, reject) =>
            {
                run();

                resolve();
            } );
        }

        public static Promise Run(Func<Promise> run)
        {
            return Promise.Run( (resolve, reject) =>
            {
                run().Then(resolve).Catch(reject);
            } );
        }

        public static Promise Run(Action<Action, Action<Exception> > run)
        {
            return new Promise(run);
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise Yield()
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.Server.QueueForExecution( () => 
            {
                return Promise.Completed;
            } );             
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise Delay(string key, TimeSpan executeIn)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.Server.QueueForExecution(key, executeIn, () => 
            {
                return Promise.Completed;
            } ); 
        }

        public static Promise WhenAll(params Promise[] promises)
        {
            if (promises == null || promises.Length == 0)
            {
                return Promise.Completed;
            }

            return Promise.Run( (resolve, reject) =>
            {
                int index = 0;

                for (int i = 0; i < promises.Length; i++)
                {
                    Exception first = null;

                    promises[i].Then( () =>
                    {
                        if (++index == promises.Length)
                        {
                            if (first == null)
                            {
                                resolve();
                            }
                            else
                            {
                                reject(first);
                            }
                        }

                    } ).Catch( (ex) =>
                    {
                        if (first == null)
                        {
                            first = ex;
                        }

                        if (++index == promises.Length)
                        {
                            reject(first);
                        }
                    } );
                }
            } );
        }

        public static Promise WhenAny(params Promise[] promises)
        {
            if (promises == null || promises.Length == 0)
            {
                return Promise.Completed;
            }

            return Promise.Run( (resolve, reject) =>
            {
                for (int i = 0; i < promises.Length; i++)
                {
                    promises[i].Then(resolve).Catch(reject);
                }
            } );
        }

        private readonly object sync = new object();

        private PromiseStatus status;

        private Exception exception;

        private List<Action> continueWithFulfilled;

        private List<Action<Exception>> continueWithRejected;

        public Promise()
        {
            status = PromiseStatus.Pending;
        }

        public Promise(Action<Action, Action<Exception> > run)
        {
            try
            {
                run( () => { TrySetResult(); }, (ex) => { TrySetException(ex); } );
            }
            catch (Exception ex)
            {
                TrySetException(ex);
            }
        }

        public bool TrySetResult()
        {
            lock (sync)
            {
                if (status == PromiseStatus.Pending)
                {
                    status = PromiseStatus.Fulfilled;

                    if (continueWithFulfilled != null)
                    {
                        foreach (var item in continueWithFulfilled)
                        {
                            item();
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

        private void AddContinueWithFulfilled(Action next)
        {
            if (continueWithFulfilled == null)
            {
                continueWithFulfilled = new List<Action>();
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

        public void Wait()
        {
            lock (sync)
            {
                if (status == PromiseStatus.Pending)
                {
                    AddContinueWithFulfilled( () =>
                    {
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
                }
                else
                {
                    if (exception != null)
                    {
                        throw exception;
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
                        AddContinueWithFulfilled( () =>
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

        public Promise Then(Action onFullfilled)
        {
            return Promise.Run( (resolve, reject) =>
            {
                lock (sync)
                {
                    if (status == PromiseStatus.Pending)
                    {
                        AddContinueWithFulfilled( () =>
                        {
                            onFullfilled();

                            resolve();
                        } );

                        AddContinueWithRejected( (ex) =>
                        {
                            reject(ex);
                        } );
                    }
                    else if (status == PromiseStatus.Fulfilled)
                    {
                        onFullfilled();

                        resolve();
                    }
                    else if (status == PromiseStatus.Rejected)
                    {
                        reject(exception);
                    }
                }
            } );
        }

        public Promise Then(Func<Promise> onFullfilled)
        {
            return Promise.Run( (resolve, reject) =>
            {
                lock (sync)
                {
                    if (status == PromiseStatus.Pending)
                    {
                        AddContinueWithFulfilled( () =>
                        {
                            onFullfilled().Then(resolve).Catch(reject);
                        } );

                        AddContinueWithRejected( (ex) =>
                        {
                            reject(ex);
                        } );
                    }
                    else if (status == PromiseStatus.Fulfilled)
                    {
                        onFullfilled().Then(resolve).Catch(reject);
                    }
                    else if (status == PromiseStatus.Rejected)
                    {
                        reject(exception);
                    }
                }
            } );
        }

        public PromiseResult<TResult> Then<TResult>(Func<TResult> onFullfilled)
        {
            return Promise.Run<TResult>( (resolve, reject) =>
            {
                lock (sync)
                {
                    if (status == PromiseStatus.Pending)
                    {
                        AddContinueWithFulfilled( () =>
                        {
                            resolve(onFullfilled() );
                        } );

                        AddContinueWithRejected( (ex) =>
                        {
                            reject(ex);
                        } );
                    }
                    else if (status == PromiseStatus.Fulfilled)
                    {
                        resolve(onFullfilled() );
                    }
                    else if (status == PromiseStatus.Rejected)
                    {
                        reject(exception);
                    }
                }
            } );
        } 

        public PromiseResult<TResult> Then<TResult>(Func<PromiseResult<TResult> > onFullfilled)
        {
            return Promise.Run<TResult>( (resolve, reject) =>
            {
                lock (sync)
                {
                    if (status == PromiseStatus.Pending)
                    {
                        AddContinueWithFulfilled( () =>
                        {
                            onFullfilled().Then(resolve).Catch(reject);
                        } );

                        AddContinueWithRejected( (ex) =>
                        {
                            reject(ex);
                        } );
                    }
                    else if (status == PromiseStatus.Fulfilled)
                    {
                        onFullfilled().Then(resolve).Catch(reject);
                    }
                    else if (status == PromiseStatus.Rejected)
                    {
                        reject(exception);
                    }
                }
            } );
        }     

        public Promise GetAwaiter()
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
                AddContinueWithFulfilled( () =>
                {
                    next();
                } );     
            
                AddContinueWithRejected( (ex) =>
                {
                    next();
                } );
            }
        }

        public void GetResult()
        {
            // Fulfilled or Rejected

            lock (sync) 
            {             
                if (exception != null)
                {
                    throw exception;
                }
            }
        }
    }

    [DebuggerStepThrough]

    public class PromiseMethodBuilder
    {
        public static PromiseMethodBuilder Create()
        {
            return new PromiseMethodBuilder();
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

        private Promise promise;

        public Promise Task
        {
            get
            {
                return promise ?? (promise = new Promise() );
            }
        }

        public void SetResult()
        {
            Task.TrySetResult();
        }

        public void SetException(Exception ex)
        {
            Task.TrySetException(ex);
        }
    }
}