using OpenTibia.Common.Structures;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

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

                    return Promise.Stop();
                }

                return next();
            } ) );

            server.CommandHandlers.Add(new InlineCommandHandler<PlayerMoveCreatureCommand>( (context, next, command) => 
            {
                if (command.ToTile.Ground == null || command.ToTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || command.ToTile.GetCreatures().Any(c => c.Block) )
                {
                    context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNotEnoughtRoom) );

                    return Promise.Stop();
                }

                return next();
            } ) );
        }

        public void Stop(Server server)
        {
            
        }
    }
}