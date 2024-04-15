﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseLogOutCommand : IncomingCommand
    {
        public ParseLogOutCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            if (Player.HasSpecialCondition(SpecialCondition.ProtectionZoneBlock) )
            {
                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouMayNotLogOutHere) );

                return Promise.Break;
            }

            return Context.AddCommand(new ShowMagicEffectCommand(Player, MagicEffectType.Puff) ).Then( () =>
            {
                return Context.AddCommand(new CreatureDestroyCommand(Player) );
            } );
        }
    }
}