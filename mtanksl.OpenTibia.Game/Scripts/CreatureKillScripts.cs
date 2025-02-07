﻿using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class CreatureKillScripts : Script
    {
        public override void Start()
        {
            Context.Server.EventHandlers.Subscribe<CreatureKillEventArgs>(new CreatureKillScriptingHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}