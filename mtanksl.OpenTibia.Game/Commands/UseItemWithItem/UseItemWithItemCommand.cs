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

        protected bool IsUseable(Item fromItem, Context context)
        {
            if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Useable) )
            {
                return false;
            }

            return true;
        }

        protected void UseItemWithItem(Item fromItem, Item toItem, Context context)
        {
            Command command = context.TransformCommand(new PlayerUseItemWithItemCommand(Player, fromItem, toItem) );

            command.Completed += (s, e) =>
            {
                base.OnCompleted(e.Context);
            };

            command.Execute(context);
        }

        protected void UseItemWithCreature(Item fromItem, Creature toCreature, Context context)
        {
            Command command = context.TransformCommand(new PlayerUseItemWithCreatureCommand(Player, fromItem, toCreature) );

            command.Completed += (s, e) =>
            {
                base.OnCompleted(e.Context);
            };

            command.Execute(context);
        }
    }
}