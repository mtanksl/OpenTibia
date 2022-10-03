using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWithCreatureWalkToTargetHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        public override bool CanHandle(Context context, PlayerUseItemWithCreatureCommand command)
        {
            if ( !command.Player.Tile.Position.IsNextTo(command.ToCreature.Tile.Position) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemWithCreatureCommand command)
        {
            context.AddCommand(new PlayerMoveItemCommand(command.Player, command.Item, command.Player.Inventory, (byte)Slot.Extra, 1) ).Then(ctx =>
            {
                return ctx.AddCommand(new WalkToUnknownPathCommand(command.Player, command.ToCreature.Tile) );

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