using System;

namespace OpenTibia.Threading
{
    public class DispatcherEvent
    {
        public DispatcherEvent(Action execute)
        {
            this.state = DispatcherExecutionState.Pending;

            this.execute = execute;
        }

        private DispatcherExecutionState state;

        public DispatcherExecutionState State
        {
            get
            {
                return state;
            }
        }

        private Action execute;

        public void Execute()
        {
            if (state == DispatcherExecutionState.Pending)
            {
                state = DispatcherExecutionState.Executing;

                OnStateChanged(state);

                execute();
                
                state = DispatcherExecutionState.Executed;

                OnStateChanged(state);
            }
        }
        
        public bool Cancel()
        {
            if (state == DispatcherExecutionState.Pending)
            {
                state = DispatcherExecutionState.Canceled;

                OnStateChanged(state);

                return true;
            }

            return false;
        }

        public event EventHandler<DispatcherStateChangedEventArgs> StateChanged;

        protected virtual void OnStateChanged(DispatcherExecutionState state)
        {
            if (StateChanged != null)
            {
                StateChanged(this, new DispatcherStateChangedEventArgs(state) );
            }
        }
    }
}