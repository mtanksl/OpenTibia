using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public abstract class UseItemWithItemCommand : Command
    {
        public UseItemWithItemCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        protected bool IsUseable(Context context, Item fromItem)
        {
            if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Useable) )
            {
                return false;
            }

            return true;
        }

        protected void UseItemWithItem(Context context, Item fromItem, Item toItem)
        {
            context.AddCommand(new PlayerUseItemWithItemCommand(Player, fromItem, toItem), ctx =>
            {
                OnComplete(context);
            } );
        }

        protected void UseItemWithCreature(Context context, Item fromItem, Creature toCreature)
        {
            context.AddCommand(new PlayerUseItemWithCreatureCommand(Player, fromItem, toCreature), ctx =>
            {
                OnComplete(context);
            } );
        }
    }
}