using System;

namespace OpenTibia.Proxy
{
    public enum DisconnetionType
    {
        Requested,

        SocketClosed,

        SocketException
    }

    public class DisconnectedEventArgs : EventArgs
    {
        public DisconnectedEventArgs(DisconnetionType type)
        {
            this.type = type;
        }

        private DisconnetionType type;

        public DisconnetionType Type
        {
            get
            {
                return type;
            }
        }
    }
}