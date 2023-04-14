using System;

namespace OpenTibia.Threading
{
    public class DispatcherStateChangedEventArgs : EventArgs
    {
        public DispatcherStateChangedEventArgs(DispatcherExecutionState state)
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