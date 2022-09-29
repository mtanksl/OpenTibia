using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public abstract class UseItemWithCreatureCommand : Command
    {
        public UseItemWithCreatureCommand(Player player)
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

        protected void UseItemWithCreature(Context context, Item fromItem, Creature toCreature)
        {
            context.AddCommand(new PlayerUseItemWithCreatureCommand(Player, fromItem, toCreature), ctx =>
            {
                base.Execute(context);
            } );
        }
    }
}