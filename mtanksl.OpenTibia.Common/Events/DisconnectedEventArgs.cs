using System;

namespace OpenTibia.Common.Events
{
    public enum DisconnetionType
    {
        Logout,

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