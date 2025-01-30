using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public interface IConnection 
    {
        string IpAddress { get; }

        IClient Client { get; set; }

        MessageProtocol MessageProtocol { get; set; }

        uint[] Keys { get; set; }

        void Send(IMessageCollection messageCollection);

        void Disconnect();
    }
}