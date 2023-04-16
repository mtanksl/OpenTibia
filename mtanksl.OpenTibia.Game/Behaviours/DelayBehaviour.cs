using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class DelayBehaviour : Behaviour
    {
        private string key;
        
        private int executeInMilliseconds;

        private Func<Promise> run;

        public DelayBehaviour(string key, int executeInMilliseconds) : this(key, executeInMilliseconds, () => { return Promise.Completed; } )
        {
           
        }

        public DelayBehaviour(string key, int executeInMilliseconds, Func<Promise> run)
        {
            this.key = key;

            this.executeInMilliseconds = executeInMilliseconds;

            this.run = run;
        }

        private Promise promise;

        public Promise Promise
        {
            get 
            {
                return promise; 
            }
        }

        public override void Start(Server server)
        {
            promise = Promise.Delay(key + GameObject.Id, executeInMilliseconds).Then( () =>
            {
                server.Components.RemoveComponent(GameObject, this);

                return run();

            } ).Catch( (ex) =>
            {
                if (ex is PromiseCanceledException)
                {
                                
                }
                else
                {
                    server.Logger.WriteLine(ex.ToString(), LogLevel.Error);
                }

                server.Components.RemoveComponent(GameObject, this);
            } );
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution(key + GameObject.Id);
        }
    }
}