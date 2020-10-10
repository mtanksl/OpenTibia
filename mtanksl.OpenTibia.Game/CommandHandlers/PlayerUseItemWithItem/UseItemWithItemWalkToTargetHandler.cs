using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWithItemWalkToTargetHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        public override bool CanHandle(PlayerUseItemWithItemCommand command, Server server)
        {
            if (command.ToItem.Container is Tile toTile && !command.Player.Tile.Position.IsNextTo(toTile.Position) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerUseItemWithItemCommand command, Server server)
        {
            List<Command> commands = new List<Command>();

            switch (command.Item.Container)
            {
                case Tile fromTile:
                    


                    commands.Add(new DelayCommand(Constants.CreatureActionSchedulerEvent(command.Player), Constants.CreatureActionSchedulerEventDelay) );

                    break;

                case Container fromContainer:

                    switch (fromContainer.GetRootContainer() )
                    {
                        case Tile fromTile:



                            commands.Add(new DelayCommand(Constants.CreatureActionSchedulerEvent(command.Player), Constants.CreatureActionSchedulerEventDelay) );

                            break;
                    }

                    break;
            }

            commands.Add(new WalkToUnknownPathCommand(command.Player, (Tile)command.ToItem.Container) );

            commands.Add(new DelayCommand(Constants.CreatureActionSchedulerEvent(command.Player), Constants.CreatureActionSchedulerEventDelay) );

            commands.Add(new CallbackCommand(context =>
            {
                return context.TransformCommand(command);
            } ) );

            return new SequenceCommand(commands.ToArray() );
        }
    }
}