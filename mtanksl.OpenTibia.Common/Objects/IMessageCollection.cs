using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IMessageCollection
    {
        void Add(IOutgoingPacket packet);

        IEnumerable<IMessage> GetMessages();
    }
}