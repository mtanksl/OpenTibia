using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class MessageCollectionFactory : IMessageCollectionFactory
    {
        public IMessageCollection Create()
        {
            return new MessageCollection();
        }
    }
}