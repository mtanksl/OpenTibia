using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public abstract class PeriodicBehaviour : Behaviour
    {
        private string key;
        
        private int executeInMilliseconds;

        public PeriodicBehaviour(string key, int executeInMilliseconds)
        {
            this.key = key;

            this.executeInMilliseconds = executeInMilliseconds;
        }

        public override void Start(Server server)
        {
            Promise.Delay(key + GameObject.Id, executeInMilliseconds).Then( () =>
            {
                Start(server);

                return Update();

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
            server.CancelQueueForExecution(key + GameObject.Id);
        }
    }
}