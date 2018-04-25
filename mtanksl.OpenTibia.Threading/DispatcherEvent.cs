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

        public bool Execute()
        {
            if (state == ExecutionState.Pending)
            {
                state = ExecutionState.Executing;
                
                execute();

                state = ExecutionState.Executed;

                return true;
            }

            return false;
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