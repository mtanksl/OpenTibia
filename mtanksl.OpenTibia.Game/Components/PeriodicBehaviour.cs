using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public abstract class PeriodicBehaviour : Behaviour
    {
        private TimeSpan executeIn;

        public PeriodicBehaviour(TimeSpan executeIn)
        {
            this.executeIn = executeIn;
        }

        public override bool IsUnique
        {
            get
            {
                return false;
            }
        }

        private string key = Guid.NewGuid().ToString();

        public override void Start()
        {
            Promise.Delay(key, executeIn).Then(Update).Then( () =>
            {
                Start();

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

        public abstract Promise Update();

        public override void Stop()
        {
            Context.Server.CancelQueueForExecution(key);
        }
    }
}