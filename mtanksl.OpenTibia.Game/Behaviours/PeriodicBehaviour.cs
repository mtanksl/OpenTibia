using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public abstract class PeriodicBehaviour : Behaviour
    {
        private int executeInMilliseconds;

        public PeriodicBehaviour(int executeInMilliseconds)
        {
            this.executeInMilliseconds = executeInMilliseconds;
        }

        public override bool IsUnique
        {
            get
            {
                return false;
            }
        }

        private string key = Guid.NewGuid().ToString();

        public override void Start(Server server)
        {
            Promise.Delay(key, executeInMilliseconds).Then(Update).Then( () =>
            {
                Start(server);

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
            } );
        }

        public abstract Promise Update();

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution(key);
        }
    }
}