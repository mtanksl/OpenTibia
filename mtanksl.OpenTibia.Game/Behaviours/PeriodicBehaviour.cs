using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class PeriodicBehaviour : Behaviour
    {
        private string key;
        
        private int executeInMilliseconds;

        private Func<Promise> run;

        public PeriodicBehaviour(string key, int executeInMilliseconds, Func<Promise> run)
        {
            this.key = key;

            this.executeInMilliseconds = executeInMilliseconds;

            this.run = run;
        }

        public override void Start(Server server)
        {
            Promise.Delay(key + GameObject.Id, executeInMilliseconds).Then( () =>
            {
                Start(server);

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