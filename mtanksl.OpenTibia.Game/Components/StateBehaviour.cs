using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public abstract class StateBehaviour : Behaviour
    {
        public enum State
        {
            None,

            Running,

            Success,

            Canceled,

            Stopped
        }

        public override void Start()
        {
            ChangeState(State.Running).Then( () =>
            {
                ChangeState(State.Success).Catch( (ex) =>
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

            } ).Catch( (ex) =>
            {
                ChangeState(State.Canceled).Catch( (ex) =>
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

        public override void Stop()
        {
            ChangeState(State.Stopped).Catch( (ex) =>
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

        private State state;

        private Promise ChangeState(State newState)
        {
            if (state == State.None)
            {
                state = State.Running;

                return OnStart();
            }
            else if (state == State.Running)
            {
                if (newState == State.Success)
                {
                    state = State.Success;

                    return OnStop(state);
                }
                else if (newState == State.Canceled)
                {
                    state = State.Canceled;

                    return OnStop(state);
                }
                else if (newState == State.Stopped)
                {
                    state = State.Stopped;

                    return OnStop(state);
                }
            }

            return Promise.Completed;
        }

        protected abstract Promise OnStart();

        protected abstract Promise OnStop(State state);
    }
}