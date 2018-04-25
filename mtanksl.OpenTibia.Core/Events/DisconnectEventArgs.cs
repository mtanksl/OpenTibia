using System;

namespace OpenTibia
{
    public enum DisconnetionType
    {
        Logout,

        SocketClosed,

        SocketException
    }

    public class DisconnectEventArgs : EventArgs
    {
        public DisconnectEventArgs(DisconnetionType type)
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