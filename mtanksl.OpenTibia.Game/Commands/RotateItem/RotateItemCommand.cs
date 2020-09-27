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

        protected bool IsRotatable(Item fromItem, Context context)
        {
            if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Rotatable) )
            {
                return false;
            }

            return true;
        }

        protected bool IsNextTo(Tile fromTile, Context context)
        {
            if ( !Player.Tile.Position.IsNextTo(fromTile.Position) )
            {
                Command command = context.TransformCommand(new WalkToUnknownPathCommand(Player, fromTile) );

                command.Completed += (s, e) =>
                {
                    context.Server.QueueForExecution(Constants.CreatureActionSchedulerEvent(Player), Constants.CreatureActionSchedulerEventDelay, this);
                };

                command.Execute(context);

                return false;
            }

            return true;
        }

        protected void RotateItem(Item fromItem, Context context)
        {
            Command command = context.TransformCommand(new PlayerRotateItemCommand(Player, fromItem) );

            command.Completed += (s, e) =>
            {
                base.OnCompleted(e.Context);
            };

            command.Execute(context);
        }
    }
}