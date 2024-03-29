﻿using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class ItemTransformScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new ItemTransformContainerCloseHandler() ); 
            
            Context.Server.CommandHandlers.AddCommandHandler(new ItemTransformTradingRejectHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}