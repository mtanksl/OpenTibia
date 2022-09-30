using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWalkToSourceHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override bool CanHandle(Context context, PlayerUseItemCommand command)
        {
            if (command.Item.Parent is Tile tile && !command.Player.Tile.Position.IsNextTo(tile.Position) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemCommand command)
        {
            context.AddCommand(new WalkToUnknownPathCommand(command.Player, (Tile)command.Item.Parent) ).Then(ctx =>
            {
                ctx.AddCommand(command);

                OnComplete(ctx);
            } );
        }
    }
}