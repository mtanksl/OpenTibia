using System;

namespace OpenTibia.Threading
{
    public class DispatcherEvent
    {
        private enum ExecutionState
        {
            Pending,

            Executing,

            Executed,

            Canceled
        }

        private Action execute;

        public DispatcherEvent(Action execute)
        {
            this.execute = execute;
        }

        private ExecutionState state = ExecutionState.Pending;

        public void Execute()
        {
            if (state == ExecutionState.Pending)
            {
                state = ExecutionState.Executing;
                
                execute();

                state = ExecutionState.Executed;
            }
        }
        
        public bool Cancel()
        {
            if (state == ExecutionState.Pending)
            {
                state = ExecutionState.Canceled;

                return true;
            }

            return false;
        }
    }
}