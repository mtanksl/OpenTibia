using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Scripts
{
    public class PlayerMoveItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.Add(new MoveItemWalkToSourceHandler() );

            Context.Server.CommandHandlers.Add(new InlineCommandHandler<PlayerMoveItemCommand>( (context, next, command) => 
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

            Context.Server.CommandHandlers.Add(new DustbinHandler() );

            Context.Server.CommandHandlers.Add(new ShallowWaterHandler() );

            Context.Server.CommandHandlers.Add(new SwampHandler() );

            Context.Server.CommandHandlers.Add(new LavaHandler() );

            Context.Server.CommandHandlers.Add(new TarHandler() );

            Context.Server.CommandHandlers.Add(new MagicForcefield2Handler() );

            Context.Server.CommandHandlers.Add(new Hole2Handler() );

            Context.Server.CommandHandlers.Add(new Pitfall2Handler() );

            Context.Server.CommandHandlers.Add(new Stairs2Handler() );

            Context.Server.CommandHandlers.Add(new CandlestickMoveHandler() );

            Context.Server.CommandHandlers.Add(new TrapMoveHandler() );

            Context.Server.CommandHandlers.Add(new SplitStackableItemHandler() );

            Context.Server.CommandHandlers.Add(new ThrowAwayContainerCloseHandler() );

            Context.Server.CommandHandlers.Add(new InventoryHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}