using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Scripts
{
    public class PlayerMoveItemScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new MoveItemWalkToSourceHandler() );

            server.CommandHandlers.Add(new InlineCommandHandler<PlayerMoveItemCommand>( (context, next, command) => 
            {
                if (command.ToContainer is Tile toTile)
                {
                    if ( !context.Server.Pathfinding.CanThrow(command.Player.Tile.Position, toTile.Position) )
                    {
                        context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotThrowThere) );

                        return Promise.FromResult(context);
                    }
                }

                return next(context);
            } ) );

            server.CommandHandlers.Add(new ThrowAwayContainerCloseHandler() );        

            server.CommandHandlers.Add(new DustbinHandler() );

            server.CommandHandlers.Add(new ShallowWaterHandler() );

            server.CommandHandlers.Add(new SwampHandler() );

            server.CommandHandlers.Add(new LavaHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}