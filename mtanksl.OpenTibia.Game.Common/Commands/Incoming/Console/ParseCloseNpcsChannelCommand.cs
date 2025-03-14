﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Commands
{
    public class ParseCloseNpcsChannelCommand : IncomingCommand
    {
        public ParseCloseNpcsChannelCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            if (Context.Server.Config.GameplayPrivateNpcSystem && Context.Server.Features.HasFeatureFlag(FeatureFlag.NpcsChannel) )
            {
                PlayerCloseNpcsChannelEventArgs e = new PlayerCloseNpcsChannelEventArgs(Player);

                foreach (var npc in Context.Server.Map.GetObserversOfTypeNpc(Player.Tile.Position) )
                {
                    if (npc.Tile.Position.CanSee(Player.Tile.Position) )
                    {
                        Context.AddEvent(npc, ObserveEventArgs.Create(npc, e) );
                    }
                }

                Context.AddEvent(Player, e);
            }
             
            return Promise.Completed;
        }
    }
}