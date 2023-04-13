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

                OnStateChange(state);

                execute();
                
                state = DispatcherExecutionState.Executed;

                OnStateChange(state);
            }
        }
        
        public bool Cancel()
        {
            if (state == DispatcherExecutionState.Pending)
            {
                state = DispatcherExecutionState.Canceled;

                OnStateChange(state);

                return true;
            }

            return false;
        }

        public event EventHandler<DispatcherStateChangeEventArgs> StateChange;

        protected virtual void OnStateChange(DispatcherExecutionState state)
        {
            if (StateChange != null)
            {
                StateChange(this, new DispatcherStateChangeEventArgs(state) );
            }
        }
    }
}