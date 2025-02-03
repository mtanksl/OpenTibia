using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Scripts
{
    public class PlayerUseItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>( (context, next, command) => 
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

                return next();
            } );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new UseItemScriptingHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new UseItemChestHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new LockerOpenHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new ContainerOpenHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new BookHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new SpellbookHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new BabySealDollHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new BlueberryBushHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new ConstructionKitHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new ClayLumpHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new CloseDoorHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new CrystalCoinHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new DiceHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new ExplosivePresentHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new FireworksRocketHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new FlaskOfDemonicBloodHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new FoodHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new GateOfExpertiseDoorHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new GoldCoinHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new LadderHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new LockedDoorHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new MusicalInstrumentHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new OpenDoorHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new PandaTeddyHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new PartyCakeHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new PartyHatHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new PartyTrumpetHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new PiggyBankHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new PlatinumCoinHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new SantaDollHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new GarlicBreadOrCookieHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new SealedDoorHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new SewerHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new SnowHeapHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new StuffedDragonHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new BlueSurpriseBagHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new RedSurpriseBagHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new SuspiciousSurpriseBagHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new UseItemTransformHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new TrapHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new WatchHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new WindowHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new PhoenixCharmHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new SolitudeCharmHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new SpiritualCharmHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new TwinSunCharmHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemCommand>(new UnityCharmHandler() );
        }

        public override void Stop()
        {

        }
    }
}