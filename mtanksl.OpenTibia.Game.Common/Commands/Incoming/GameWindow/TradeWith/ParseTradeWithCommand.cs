using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public abstract class ParseTradeWithCommand : IncomingCommand
    {
        public ParseTradeWithCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        protected bool IsPickupable(Item fromItem)
        {
            if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Pickupable) )
            {
                return false;
            }

            return true;
        }

        protected bool IsMoveable(Item fromItem)
        {
            if ( fromItem.Metadata.Flags.Is(ItemMetadataFlags.NotMoveable) )
            {
                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.YouCanNotMoveThisObject) );

                return false;
            }

            return true;
        }
    }
}