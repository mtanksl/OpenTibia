using System;

namespace OpenTibia.Threading
{
    public class DispatcherEvent
    {
        private enum DispatcherEventState
        {
            Pending,

            Executing,

            Executed,

            Canceled
        }

        private DispatcherEventState state;

        private Action execute;

        public DispatcherEvent(Action execute)
        {
            this.state = DispatcherEventState.Pending;

            this.execute = execute;
        }

        public void Execute()
        {
            if (state == DispatcherEventState.Pending)
            {
                state = DispatcherEventState.Executing;

                execute();
                
                state = DispatcherEventState.Executed;
            }
        }
        
        public bool Cancel()
        {
            if (state == DispatcherEventState.Pending)
            {
                state = DispatcherEventState.Canceled;

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