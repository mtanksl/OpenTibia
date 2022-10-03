using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWithItemWalkToTargetHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        public override bool CanHandle(Context context, PlayerUseItemWithItemCommand command)
        {
            if (command.ToItem.Parent is Tile toTile && !command.Player.Tile.Position.IsNextTo(toTile.Position) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemWithItemCommand command)
        {
            context.AddCommand(new PlayerMoveItemCommand(command.Player, command.Item, command.Player.Inventory, (byte)Slot.Extra, 1) ).Then(ctx =>
            {
                return ctx.AddCommand(new WalkToUnknownPathCommand(command.Player, (Tile)command.ToItem.Parent) );

            } ).Then(ctx =>
            {                
                //TODO: Check if item has moved

                return ctx.AddCommand(command);

            } ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}