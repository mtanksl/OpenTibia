using OpenTibia.Common.Structures;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Scripts
{
    public class PlayerUseItemWithCreatureScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithCreatureCommand>(new UseItemWithCreatureScriptingHandler(true) );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithCreatureCommand>(new RunesHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithCreatureCommand>( (context, next, command) =>
            {
                if ( !command.Player.Tile.Position.IsNextTo(command.ToCreature.Tile.Position) )
                {
                     Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.TooFarAway) );

                    return Promise.Break;
                }

                return next();
            } );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithCreatureCommand>(new UseItemWithCreatureScriptingHandler(false) );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithCreatureCommand>(new AntidotePotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithCreatureCommand>(new SmallHealthPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithCreatureCommand>(new HealthPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithCreatureCommand>(new GreatHealthPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithCreatureCommand>(new StrongHealthPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithCreatureCommand>(new UltimateHealthPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithCreatureCommand>(new ManaPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithCreatureCommand>(new GreatManaPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithCreatureCommand>(new StrongManaPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithCreatureCommand>(new GreatSpiritPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithCreatureCommand>(new FluidItemHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerUseItemWithCreatureCommand>(new VoodooDollHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}