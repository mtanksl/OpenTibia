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
            Context.Server.CommandHandlers.AddCommandHandler(new UseItemWithItemScriptingHandler(true) );

            Context.Server.CommandHandlers.AddCommandHandler(new Runes2Handler() );

            Context.Server.CommandHandlers.AddCommandHandler(new FishingRodHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithItemCommand>( (context, next, command) =>
            {
                if (command.ToItem.Parent is Tile toTile && !command.Player.Tile.Position.IsNextTo(toTile.Position) )
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TooFarAway) );

                    return Promise.Break;
                }

                return next();
            } );

            Context.Server.CommandHandlers.AddCommandHandler(new UseItemWithItemScriptingHandler(false) );

            Context.Server.CommandHandlers.AddCommandHandler(new BakingTrayWithDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new BakingTrayWithGarlicDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new BunchOfSugarCaneHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new FireBugHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new FlourHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new FluidItem2Handler() );

            Context.Server.CommandHandlers.AddCommandHandler(new JuiceSqueezerHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new KeyHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new KnifeHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new LumpOfCakeDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new LumpOfChocolateDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new LumpOfDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new LumpOfHolyWaterDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new LumpOfGarlicDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new MacheteHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ObsidianKnifeHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PickHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new RopeHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ScytheHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ShovelHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SickleHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new WheatHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}