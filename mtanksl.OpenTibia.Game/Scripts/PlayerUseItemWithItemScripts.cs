using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Scripts
{
    public class PlayerUseItemWithItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new UseItemWithItemScriptingHandler(true) );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new Runes2Handler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new FishingRodHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>( (context, next, command) =>
            {
                if (command.ToItem.Parent is Tile toTile && !command.Player.Tile.Position.IsNextTo(toTile.Position) )
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.TooFarAway) );

                    return Promise.Break;
                }

                return next();
            } );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new UseItemWithItemScriptingHandler(false) );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new BakingTrayWithDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new BakingTrayWithGarlicDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new BlessedWoodenStakeHandler() );
            
            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new BunchOfSugarCaneHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new CrowbarHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new FireBugHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new FlourHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new FluidItem2Handler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new JuiceSqueezerHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new KeyHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new KnifeHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new LumpOfCakeDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new LumpOfChocolateDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new LumpOfDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new LumpOfHolyWaterDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new LumpOfGarlicDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new MacheteHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new ObsidianKnifeHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new PickHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new RopeHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new SawHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new ScytheHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new ShovelHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new SickleHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>(new WheatHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}