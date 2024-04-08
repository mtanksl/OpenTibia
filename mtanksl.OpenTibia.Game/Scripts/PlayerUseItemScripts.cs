using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Scripts
{
    public class PlayerUseItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new UseItemWalkToSourceHandler() ); //TODO: Re-validate rules for incoming packet
            
            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>( (context, next, command) => 
            {
                if (command.Item.Parent is Tile tile)
                {
                    bool hangable = false;

                    bool? vertical = null;

                    foreach (var item in tile.GetItems() )
                    {
                        if (item.Metadata.Flags.Is(ItemMetadataFlags.Hangable) )
                        {
                            hangable = true;
                        } 

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

                    if (hangable)
                    {
                        if (vertical == true)
                        {
                            if (command.Player.Tile.Position.X + 1 == tile.Position.X)
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );

                                return Promise.Break;
                            }
                        }
                        else if (vertical == false)
                        {
                            if (command.Player.Tile.Position.Y + 1 == tile.Position.Y)
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );

                                return Promise.Break;
                            }
                        }
                    }
                }

                return next();
            } );

            Context.Server.CommandHandlers.AddCommandHandler(new UseItemScriptingHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new UseItemChestHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new LockerOpenHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ContainerOpenHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new BookHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SpellbookHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new BabySealDollHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new BlueberryBushHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ConstructionKitHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ClayLumpHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new CloseDoorHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new CrystalCoinHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new DiceHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ExplosivePresentHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new FireworksRocketHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new FlaskOfDemonicBloodHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new FoodHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new GateOfExpertiseHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new GoldCoinHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new LadderHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new LockedDoorHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new MusicalInstrumentHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new OpenDoorHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PandaTeddyHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PartyCakeHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PartyHatHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PartyTrumpetHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PiggyBankHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PlatinumCoinHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SantaDollHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new GarlicBreadOrCookie() );

            Context.Server.CommandHandlers.AddCommandHandler(new SealedDoorHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SewerHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SnowHeapHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new StuffedDragonHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SurpriseBagBlueHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SurpriseBagRedHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SurpriseBagSuspiciousHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new UseItemTransformHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new TrapHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new WatchHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new WindowHandler() );
        }

        public override void Stop()
        {

        }
    }
}