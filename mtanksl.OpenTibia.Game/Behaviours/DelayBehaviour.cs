using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class DelayBehaviour : Behaviour
    {
        protected string key = Guid.NewGuid().ToString();

        private int executeInMilliseconds;

        public DelayBehaviour(int executeInMilliseconds)
        {
            this.executeInMilliseconds = executeInMilliseconds;
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
            promise = Promise.Delay(key, executeInMilliseconds).Then( () =>
            {
                return Update();

            } ).Then( () =>
            {
                server.Components.RemoveComponent(GameObject, this);

                return Promise.Completed;
                
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

        public virtual Promise Update()
        {
            return Promise.Completed;
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution(key);
        }
    }
}