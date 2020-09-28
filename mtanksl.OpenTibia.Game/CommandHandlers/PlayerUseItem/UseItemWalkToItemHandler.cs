using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWalkToItemHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override bool CanHandle(PlayerUseItemCommand command, Server server)
        {
            if (command.Item.Container is Tile tile && !command.Player.Tile.Position.IsNextTo(tile.Position) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerUseItemCommand command, Server server)
        {
            return new SequenceCommand(

                new WalkToUnknownPathCommand(command.Player, (Tile)command.Item.Container),

                new DelayCommand(Constants.CreatureActionSchedulerEvent(command.Player), Constants.CreatureActionSchedulerEventDelay),

                new ChainCommand(context =>
                {
                    return context.TransformCommand(command);
                } ) );
        }
    }
}