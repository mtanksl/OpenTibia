using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Scripts
{
    public class PlayerMoveItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>( (context, next, command) => 
            {
                if (command.Pathfinding && command.ToContainer is Tile toTile && !Context.Server.Pathfinding.CanThrow(command.Player.Tile.Position, toTile.Position) )
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotThrowThere) );

                    return Promise.Break;
                }

                return next();
            } );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>( (context, next, command) => 
            {
                if (command.Item.Metadata.Flags.Is(ItemMetadataFlags.Hangable) && command.Item.Parent is Tile tile)
                {
                    bool? vertical = null;

                    foreach (var item in tile.GetItems() )
                    {
                        if (item.Metadata.Flags.Is(ItemMetadataFlags.Vertical) )
                        {
                            if (vertical == null)
                            {
                                vertical = true;
                            }
                        }

                        if (item.Metadata.Flags.Is(ItemMetadataFlags.Horizontal) )
                        {
                            if (vertical == null)
                            {
                                vertical = false;
                            }
                        }
                    }

                    if (vertical == true)
                    {
                        if (command.Player.Tile.Position.X + 1 == tile.Position.X)
                        {
                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotMoveThisObject) );

                            return Promise.Break;
                        }
                    }
                    else if (vertical == false)
                    {
                        if (command.Player.Tile.Position.Y + 1 == tile.Position.Y)
                        {
                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotMoveThisObject) );

                            return Promise.Break;
                        }
                    }
                }

                return next();
            } );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new MoveItemScriptingHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new MoveItemChestHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new MoveItemHouseTileHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new WaterBallsHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new DustbinHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new ShallowWaterHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new SwampHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new LavaHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new TarHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new MagicForcefield2Handler() );
                        
            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new Hole2Handler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new Pitfall2Handler() );
           
            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new Stairs2Handler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new CandlestickMoveHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new TrapMoveHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new LetterHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new ParcelHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new SafeHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new InventoryDressHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new TapestryHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new InventoryCapacityHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>(new SplitStackableItemHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}