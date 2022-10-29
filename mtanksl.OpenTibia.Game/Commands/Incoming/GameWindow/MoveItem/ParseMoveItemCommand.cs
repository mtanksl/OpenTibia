using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public abstract class ParseMoveItemCommand : Command
    {
        public ParseMoveItemCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        protected bool IsMoveable(Context context, Item fromItem, byte count)
        {
            if ( fromItem.Metadata.Flags.Is(ItemMetadataFlags.NotMoveable) )
            {
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotMoveThisObject) );

                return false;
            }

            if (fromItem is StackableItem stackableItem)
            {
                if (count < 1 || count > stackableItem.Count)
                {
                    return false;
                }
            }
            else
            {
                if (count != 1)
                {
                    return false;
                }
            }

            return true;
        }

        protected bool IsMoveable(Context context, Creature fromCreature)
        {
            return true;
        }
    }
}