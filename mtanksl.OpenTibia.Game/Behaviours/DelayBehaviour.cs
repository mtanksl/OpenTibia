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
            promise = Promise.Delay(key + GameObject.Id, executeInMilliseconds).Then( () =>
            {
                server.Components.RemoveComponent(GameObject, this);

                return Promise.Completed;

            } ).Catch( (ex) =>
            {
                server.Components.RemoveComponent(GameObject, this);
            } );
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution(key + GameObject.Id);
        }
    }
}