using OpenTibia.Common.Structures;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Scripts
{
    public class PlayerMoveCreatureScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveCreatureCommand>( (context, next, command) =>
            {
                if (command.ToTile.Ground == null || command.ToTile.NotWalkable || command.ToTile.BlockPathFinding || command.ToTile.Block)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.ThereIsNotEnoughtRoom) );

                    return Promise.Break;
                }

                if ( !command.Creature.Tile.Position.IsNextTo(command.ToTile.Position) )
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.DestinationIsOutOfReach) );

                    return Promise.Break;
                }

                return next();
            } );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveCreatureCommand>(new MoveCreatureScriptingHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}