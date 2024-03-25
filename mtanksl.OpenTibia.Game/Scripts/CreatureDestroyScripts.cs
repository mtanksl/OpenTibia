﻿using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class CreatureDestroyScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new NpcDestroyTradingRejectHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PlayerDestroyContainerCloseHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PlayerDestroyTradingRejectHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PlayerDestroyNpcTradingRejectHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PlayerDestroyWindowCloseHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PlayerDestroyChannelCloseHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PlayerDestroyRuleViolationCloseHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}