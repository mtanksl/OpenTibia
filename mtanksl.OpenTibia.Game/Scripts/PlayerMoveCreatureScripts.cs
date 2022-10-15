using OpenTibia.Common.Structures;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Scripts
{
    public class PlayerMoveCreatureScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new MoveCreatureWalkToSourceHandler() );

            server.CommandHandlers.Add(new InlineCommandHandler<PlayerMoveCreatureCommand>( (context, next, command) => 
            {
                if ( !command.Creature.Tile.Position.IsNextTo(command.ToTile.Position) )
                {
                    context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.DestinationIsOutOfReach) );

                    return Promise.FromResult(context);
                }

                return next(context);
            } ) );
        }

        public void Stop(Server server)
        {
            
        }
    }
}