using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWithCreatureWalkToSourceHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        public override bool CanHandle(PlayerUseItemWithCreatureCommand command, Server server)
        {
            if (command.Item.Container is Tile tile && !command.Player.Tile.Position.IsNextTo(tile.Position) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerUseItemWithCreatureCommand command, Server server)
        {
            List<Command> commands = new List<Command>();

            commands.Add(new WalkToUnknownPathCommand(command.Player, (Tile)command.Item.Container) );

            commands.Add(new DelayCommand(Constants.CreatureActionSchedulerEvent(command.Player), Constants.CreatureActionSchedulerEventDelay) );

            commands.Add(new CallbackCommand(context =>
            {
                return context.TransformCommand(command);
            } ) );

            return new SequenceCommand(commands.ToArray() );
        }
    }
}