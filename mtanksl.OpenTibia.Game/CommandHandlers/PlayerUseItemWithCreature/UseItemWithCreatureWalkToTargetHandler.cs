using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWithCreatureWalkToTargetHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        public override bool CanHandle(PlayerUseItemWithCreatureCommand command, Server server)
        {
            if ( !command.Player.Tile.Position.IsNextTo(command.ToCreature.Tile.Position) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerUseItemWithCreatureCommand command, Server server)
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

            commands.Add(new WalkToUnknownPathCommand(command.Player, command.ToCreature.Tile) );

            commands.Add(new DelayCommand(Constants.CreatureActionSchedulerEvent(command.Player), Constants.CreatureActionSchedulerEventDelay) );

            commands.Add(new ChainCommand(context =>
            {
                return context.TransformCommand(command);
            } ) );

            return new SequenceCommand(commands.ToArray() );
        }
    }
}