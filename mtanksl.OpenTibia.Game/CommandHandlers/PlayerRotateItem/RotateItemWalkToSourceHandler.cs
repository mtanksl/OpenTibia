using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class RotateItemWalkToSourceHandler : CommandHandler<PlayerRotateItemCommand>
    {
        public override bool CanHandle(Context context, PlayerRotateItemCommand command)
        {
            if (command.Item.Parent is Tile tile && !command.Player.Tile.Position.IsNextTo(tile.Position) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerRotateItemCommand command)
        {
            context.AddCommand(new WalkToUnknownPathCommand(command.Player, (Tile)command.Item.Parent) ).Then(ctx =>
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