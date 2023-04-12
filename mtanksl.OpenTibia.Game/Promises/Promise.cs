using System;
using System.Diagnostics;

namespace OpenTibia.Game.Commands
{
    [DebuggerStepThrough]
    public class Promise
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
            reject(new PromiseCanceledException() );
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
            return Promise.Run<TResult>( [DebuggerStepThrough] (resolve, reject) =>
            {
                resolve(result);
            } );
        }

        public static PromiseResult<TResult> Run<TResult>(Func<TResult> run)
        {
            return Promise.Run<TResult>( [DebuggerStepThrough] (resolve, reject) =>
            {
                var result = run();

                resolve(result);
            } );
        }

        public static PromiseResult<TResult> Run<TResult>(Func<PromiseResult<TResult>> run)
        {
            return Promise.Run<TResult>( [DebuggerStepThrough] (resolve, reject) =>
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
            return Promise.Run( [DebuggerStepThrough] (resolve, reject) =>
            {
                reject(exception);
            } );
        }

        public static Promise Run(Action run)
        {
            return Promise.Run( [DebuggerStepThrough] (resolve, reject) =>
            {
                run();

                resolve();
            } );
        }

        public static Promise Run(Func<Promise> run)
        {
            return Promise.Run( [DebuggerStepThrough] (resolve, reject) =>
            {
                run().Then(resolve).Catch(reject);
            } );
        }

        public static Promise Run(Action<Action, Action<Exception> > run)
        {
            return new Promise(run);
        }

        public static Promise Yield(Server server)
        {
            return Promise.Run( [DebuggerStepThrough] (resolve, reject) =>
            {
                server.QueueForExecution( () =>
                {
                    resolve();

                    return Promise.Completed;
                } );               
            } );            
        }

        public static Promise Delay(Server server, string key, int executeInMilliseconds)
        {
            return Promise.Run( [DebuggerStepThrough] (resolve, reject) =>
            {
                server.QueueForExecution(key, executeInMilliseconds, () =>
                {
                    resolve();

                    return Promise.Completed;
                } );               
            } ); 
        }

        public static Promise WhenAll(params Promise[] promises)
        {
            if (promises == null || promises.Length == 0)
            {
                return Promise.Completed;
            }

            return Promise.Run( [DebuggerStepThrough] (resolve, reject) =>
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

                    } ).Catch(reject);
                }
            } );
        }

        public static Promise WhenAny(params Promise[] promises)
        {
            if (promises == null || promises.Length == 0)
            {
                return Promise.Completed;
            }

            return Promise.Run( [DebuggerStepThrough] (resolve, reject) =>
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

                    } ).Catch(reject);
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
            try
            {
                run(Resolve, Reject);
            }
            catch (Exception ex)
            {
                Reject(ex);
            }
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

        public Promise Catch(Action<Exception> onRejected)
        {
            return Promise.Run( [DebuggerStepThrough] (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = () =>
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

        public Promise Then(Action onFullfilled)
        {
            return Promise.Run( [DebuggerStepThrough] (resolve, reject) =>
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
                    reject(this.exception);
                }
            } );
        }

        public Promise Then(Func<Promise> onFullfilled)
        {
            return Promise.Run( [DebuggerStepThrough] (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = () =>
                    {
                        onFullfilled().Then(resolve).Catch(reject);
                    };

                    this.continueWithRejected = (e) =>
                    {
                        reject(e);
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    onFullfilled().Then(resolve).Catch(reject);
                }
                else if (this.status == PromiseStatus.Rejected)
                {
                    reject(this.exception);
                }
            } );
        }

        public PromiseResult<TResult> Then<TResult>(Func<TResult> onFullfilled)
        {
            return Promise.Run<TResult>( [DebuggerStepThrough] (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = () =>
                    {
                        var result = onFullfilled();

                        resolve(result);
                    };

                    this.continueWithRejected = (e) =>
                    {
                        reject(e);
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    var result = onFullfilled();

                    resolve(result);
                }
                else if (this.status == PromiseStatus.Rejected)
                {
                    reject(this.exception);
                }
            } );
        } 

        public PromiseResult<TResult> Then<TResult>(Func<PromiseResult<TResult> > onFullfilled)
        {
            return Promise.Run<TResult>( [DebuggerStepThrough] (resolve, reject) =>
            {
                if (this.status == PromiseStatus.Pending)
                {
                    this.continueWithFulfilled = () =>
                    {
                        onFullfilled().Then(resolve).Catch(reject);
                    };

                    this.continueWithRejected = (e) =>
                    {
                        reject(e);
                    };
                }
                else if (this.status == PromiseStatus.Fulfilled)
                {
                    onFullfilled().Then(resolve).Catch(reject);
                }
                else if (this.status == PromiseStatus.Rejected)
                {
                    reject(this.exception);
                }
            } );
        }     
    }
}