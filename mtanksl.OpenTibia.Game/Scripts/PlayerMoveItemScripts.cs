using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Scripts
{
    public class PlayerMoveItemScripts : Script
    {
        public override void Start(Server server)
        {
            server.CommandHandlers.Add(new MoveItemWalkToSourceHandler() );

            server.CommandHandlers.Add(new InlineCommandHandler<PlayerMoveItemCommand>( (context, next, command) => 
            {
                if (command.ToContainer is Tile toTile)
                {
                    if (command.Pathfinding && !context.Server.Pathfinding.CanThrow(command.Player.Tile.Position, toTile.Position) )
                    {
                        context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotThrowThere) );

                        return Promise.Break;
                    }
                }
                else if (command.ToContainer is Inventory toInventory)
                {
                    if ( !command.Item.Metadata.Flags.Is(ItemMetadataFlags.Pickupable) )
                    {
                        context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotTakeThisObject) );

                        return Promise.Break;
                    }
                }
                else if (command.ToContainer is Container toContainer)
                {
                    if ( !command.Item.Metadata.Flags.Is(ItemMetadataFlags.Pickupable) )
                    {
                        context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotTakeThisObject) );

                        return Promise.Break;
                    }

                    if ( toContainer.IsContentOf(command.Item) )
                    {
                        context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThisIsImpossible) );

                        return Promise.Break;
                    }
                }

                return next();
            } ) );

            server.CommandHandlers.Add(new DustbinHandler() );

            server.CommandHandlers.Add(new ShallowWaterHandler() );

            server.CommandHandlers.Add(new SwampHandler() );

            server.CommandHandlers.Add(new LavaHandler() );

            server.CommandHandlers.Add(new TarHandler() );

            server.CommandHandlers.Add(new MagicForcefield2Handler() );

            server.CommandHandlers.Add(new Hole2Handler() );

            server.CommandHandlers.Add(new Pitfall2Handler() );

            server.CommandHandlers.Add(new Stairs2Handler() );

            server.CommandHandlers.Add(new CandlestickMoveHandler() );

            server.CommandHandlers.Add(new TrapMoveHandler() );

            server.CommandHandlers.Add(new SplitStackableItemHandler() );

            server.CommandHandlers.Add(new ThrowAwayContainerCloseHandler() );
        }

        public override void Stop(Server server)
        {
            
        }
    }
}