using OpenTibia.Common.Structures;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Scripts
{
    public class PlayerMoveCreatureScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new MoveCreatureWalkToSourceHandler() ); //TODO: Re-validate rules for incoming packet

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveCreatureCommand>( (context, next, command) =>
            {
                if (command.ToTile.Ground == null || !command.ToTile.CanWalk)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNotEnoughtRoom) );

                    return Promise.Break;
                }

                if ( !command.Creature.Tile.Position.IsNextTo(command.ToTile.Position) )
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.DestinationIsOutOfReach) );

                    return Promise.Break;
                }

                return next();
            } );

            Context.Server.CommandHandlers.AddCommandHandler(new MoveCreatureScriptingHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}