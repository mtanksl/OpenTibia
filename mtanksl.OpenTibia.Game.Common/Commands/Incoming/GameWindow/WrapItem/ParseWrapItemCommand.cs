using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public abstract class ParseWrapItemCommand : IncomingCommand
    {
        public ParseWrapItemCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        protected bool IsWrapable(Item fromItem)
        {
            if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Wrappable) &&
                 !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Unwrappable) )
            {
                return false;
            }

            return true;
        }
    }
}