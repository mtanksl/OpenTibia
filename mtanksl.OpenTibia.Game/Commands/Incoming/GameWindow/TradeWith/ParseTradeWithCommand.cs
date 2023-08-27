using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public abstract class ParseTradeWithCommand : Command
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
    }
}