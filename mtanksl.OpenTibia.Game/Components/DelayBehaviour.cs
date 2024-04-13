using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public abstract class DelayBehaviour : StateBehaviour
    {
        private TimeSpan executeIn;

        public DelayBehaviour(TimeSpan executeIn)
        {
            this.executeIn = executeIn;
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
            return Promise.Delay(key, executeIn);
        }

        protected override Promise OnStop(State state)
        {
            switch (state)
            {
                case State.Success:
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