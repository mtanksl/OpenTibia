using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class DecayBehaviour : Behaviour
    {
        private string key;

        private int executeInMilliseconds;

        public DecayBehaviour(int executeInMilliseconds)
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
            key = "Decay_Behaviour_" + GameObject.Id;

            promise = Promise.Delay(server, key, executeInMilliseconds);
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution(key);
        }
    }
}