using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class DelayBehaviour : StateBehaviour
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

        public string Key
        {
            get
            {
                return key;
            }
        }

        protected override Promise OnStart()
        {
            promise = Promise.Delay(key, executeIn);

            return promise;
        }

        protected override Promise OnStop(State state)
        {
            switch (state)
            {
                case State.Success:
                               
                    Context.Current.Server.GameObjectComponents.RemoveComponent(GameObject, this);

                    break;

                case State.Canceled:
                             
                    Context.Current.Server.GameObjectComponents.RemoveComponent(GameObject, this);

                    break;

                case State.Stopped:
                             
                    Context.Current.Server.CancelQueueForExecution(key);

                    break;
            }

            return Promise.Completed;
        }
    }
}