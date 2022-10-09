using System;

namespace OpenTibia.Common.Events
{
    public class DisconnectedEventArgs : EventArgs
    {
        public DisconnectedEventArgs(DisconnectionType type)
        {
            this.type = type;
        }

        private DisconnectionType type;

        public DisconnectionType Type
        {
            get
            {
                return type;
            }
        }
    }

    public enum DisconnectionType
    {
        Requested,

        SocketClosed,

        SocketException
    }
}