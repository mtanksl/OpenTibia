using System;

namespace OpenTibia.Threading
{
    public class DispatcherEvent
    {
        private enum DispatcheEventState
        {
            Pending,

            Executing,

            Executed,

            Canceled
        }

        private DispatcheEventState state;

        private Action execute;

        public DispatcherEvent(Action execute)
        {
            this.state = DispatcheEventState.Pending;

            this.execute = execute;
        }

        public void Execute()
        {
            if (state == DispatcheEventState.Pending)
            {
                state = DispatcheEventState.Executing;

                execute();
                
                state = DispatcheEventState.Executed;
            }
        }
        
        public bool Cancel()
        {
            if (state == DispatcheEventState.Pending)
            {
                state = DispatcheEventState.Canceled;

                OnCanceled();

                return true;
            }

            return false;
        }

        public event EventHandler<DispatcherEventCanceledEventArgs> Canceled;

        protected virtual void OnCanceled()
        {
            if (Canceled != null)
            {
                Canceled(this, new DispatcherEventCanceledEventArgs() );
            }
        }
    }
}