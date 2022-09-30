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
            OnComplete(context);
        }
    }
}