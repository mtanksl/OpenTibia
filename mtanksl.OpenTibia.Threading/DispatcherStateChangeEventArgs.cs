using System;

namespace OpenTibia.Threading
{
    public class DispatcherStateChangeEventArgs : EventArgs
    {
        public DispatcherStateChangeEventArgs(DispatcherExecutionState state)
        {
            this.state = state;
        }

        private DispatcherExecutionState state;

        public DispatcherExecutionState State
        {
            get
            {
                return state;
            }
        }
    }
}