using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class DelayBehaviour : Behaviour
    {
        private TimeSpan executeIn;

        public DelayBehaviour(TimeSpan executeIn)
        {
            this.executeIn = executeIn;
        }

        private Promise promise;

        public Promise Promise
        {
            get 
            {
                return promise; 
            }
        }

        private string key = Guid.NewGuid().ToString();

        public override void Start()
        {
            promise = Promise.Delay(key, executeIn).Then(Update).Then( () =>
            {
                Context.Server.GameObjectComponents.RemoveComponent(GameObject, this);

                return Promise.Completed;
                
            } ).Catch( (ex) =>
            {
                if (ex is PromiseCanceledException)
                {
                    //
                }
                else
                {
                    Context.Server.Logger.WriteLine(ex.ToString(), LogLevel.Error);
                }
            } );
        }

        public virtual Promise Update()
        {
            return Promise.Completed;
        }

        public override void Stop()
        {
            Context.Server.CancelQueueForExecution(key);
        }
    }
}