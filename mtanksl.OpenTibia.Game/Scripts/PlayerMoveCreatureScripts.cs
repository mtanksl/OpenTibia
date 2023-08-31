using OpenTibia.Common.Structures;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Scripts
{
    public class PlayerMoveCreatureScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new MoveCreatureWalkToSourceHandler() );

            //TODO: Re-validate rules for incoming packet

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveCreatureCommand>( (context, next, command) => 
            {
                if ( !command.Creature.Tile.Position.IsNextTo(command.ToTile.Position) )
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.DestinationIsOutOfReach) );

                    return Promise.Break;
                }

                return next();
            } );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveCreatureCommand>( (context, next, command) => 
            {
                if (command.ToTile.Ground == null || command.ToTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || command.ToTile.GetCreatures().Any(c => c.Block) )
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNotEnoughtRoom) );

                    return Promise.Break;
                }

                return next();
            } );
        }

        public override void Stop()
        {
            
        }
    }
}