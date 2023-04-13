using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public abstract class ParseRotateItemCommand : Command
    {
        public ParseRotateItemCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        protected bool IsRotatable(Item fromItem)
        {
            if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Rotatable) )
            {
                return false;
            }

            return true;
        }
    }
}