using OpenTibia.Common.Structures;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Scripts
{
    public class PlayerUseItemWithCreatureScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new UseItemWithCreatureScriptingHandler(true) );

            Context.Server.CommandHandlers.AddCommandHandler(new RunesHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithCreatureCommand>( (context, next, command) =>
            {
                if ( !command.Player.Tile.Position.IsNextTo(command.ToCreature.Tile.Position) )
                {
                     Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TooFarAway) );

                    return Promise.Break;
                }

                return next();
            } );

            Context.Server.CommandHandlers.AddCommandHandler(new UseItemWithCreatureScriptingHandler(false) );

            Context.Server.CommandHandlers.AddCommandHandler(new SmallHealthPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new HealthPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new GreatHealthPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new StrongHealthPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new UltimateHealthPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ManaPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new GreatManaPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new StrongManaPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new GreatSpiritPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new FluidItemHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new VoodooDollHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}