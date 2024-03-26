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
            Context.Server.CommandHandlers.AddCommandHandler(new MoveItemWalkToSourceHandler() );

            //TODO: Re-validate rules for incoming packet

            Context.Server.CommandHandlers.AddCommandHandler<PlayerMoveItemCommand>( (context, next, command) => 
            {
                if (command.ToContainer is Tile toTile)
                {
                    if (command.Pathfinding && !Context.Server.Pathfinding.CanThrow(command.Player.Tile.Position, toTile.Position) )
                    {
                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotThrowThere) );

                        return Promise.Break;
                    }
                }
                else if (command.ToContainer is Inventory toInventory)
                {
                    if ( !command.Item.Metadata.Flags.Is(ItemMetadataFlags.Pickupable) )
                    {
                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotTakeThisObject) );

                        return Promise.Break;
                    }

                    //TODO: Capacity
                }
                else if (command.ToContainer is Container toContainer)
                {
                    if ( !command.Item.Metadata.Flags.Is(ItemMetadataFlags.Pickupable) )
                    {
                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotTakeThisObject) );

                        return Promise.Break;
                    }

                    if ( toContainer.IsContentOf(command.Item) )
                    {
                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThisIsImpossible) );

                        return Promise.Break;
                    }

                    if ( !(command.Item.Root() is Safe) && toContainer.Root() is Safe)
                    {
                        foreach (var pair in Context.Server.Lockers.GetIndexedLockers(command.Player.DatabasePlayerId) )
                        {
                            Locker locker = pair.Value;

                            if (toContainer.IsContentOf(locker) )
                            {
                                if (command.Item is Container container)
                                {
                                    if (Sum(locker) + Sum(container) >= Constants.MaxDepotItems)
                                    {
                                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YourDepotIsFull) );

                                        return Promise.Break;
                                    }
                                }
                                else
                                {
                                    if (Sum(locker) >= Constants.MaxDepotItems)
                                    {
                                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YourDepotIsFull) );

                                        return Promise.Break;
                                    }
                                }
                          
                                break;
                            }
                        }
                    }

                    //TODO: Capacity
                }

                return next();

                static int Sum(IContainer parent)
                {
                    int sum = 0;

                    foreach (Item content in parent.GetContents() )
                    {                          
                        sum += 1;

                        if (content is Container container)
                        {
                            sum += Sum(container);
                        }
                    }

                    return sum;
                }
            } );

            Context.Server.CommandHandlers.AddCommandHandler(new MoveItemScriptingHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ParcelHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new LetterHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new DustbinHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ShallowWaterHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SwampHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new LavaHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new TarHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new MagicForcefield2Handler() );

            Context.Server.CommandHandlers.AddCommandHandler(new Hole2Handler() );

            Context.Server.CommandHandlers.AddCommandHandler(new Pitfall2Handler() );

            Context.Server.CommandHandlers.AddCommandHandler(new Stairs2Handler() );

            Context.Server.CommandHandlers.AddCommandHandler(new CandlestickMoveHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new TrapMoveHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SplitStackableItemHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new InventoryHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new RingHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new HelmetOfTheDeepHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}