using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public abstract class RotateItemCommand : Command
    {
        public RotateItemCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        protected bool IsRotatable(Context context, Item fromItem)
        {
            if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Rotatable) )
            {
                return false;
            }

            return true;
        }

        protected void RotateItem(Context context, Item fromItem)
        {
            context.AddCommand(new PlayerRotateItemCommand(Player, fromItem) ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}