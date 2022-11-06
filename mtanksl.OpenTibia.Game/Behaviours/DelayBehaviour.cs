using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class DelayBehaviour : Behaviour
    {
        private string key;
        
        private int executeInMilliseconds;

        public DelayBehaviour(string key, int executeInMilliseconds)
        {
            this.key = key;

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
            promise = Promise.Delay(server, key + GameObject.Id, executeInMilliseconds);
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution(key + GameObject.Id);
        }
    }
}