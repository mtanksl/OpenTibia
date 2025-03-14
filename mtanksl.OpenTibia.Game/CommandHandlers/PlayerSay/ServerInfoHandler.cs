﻿using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Globalization;

namespace OpenTibia.Game.CommandHandlers
{
    public class ServerInfoHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("!serverinfo") ) 
            {
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.PurpleDefault, command.Message) );

                if (Context.Server.Config.GameplayWorldType == WorldType.Pvp)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "World type: pvp") );

                    if (Context.Server.Config.GameplayProtectionLevel > 0)
                    {
                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Protection level: " + Context.Server.Config.GameplayProtectionLevel) );
                    }
                }
                else
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "World type: non-pvp") );
                }
         
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Experience rate: " + Context.Server.Config.GameplayExperienceRate.ToString(CultureInfo.InvariantCulture) ) );

                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Skill rate: " + Context.Server.Config.GameplaySkillRate.ToString(CultureInfo.InvariantCulture) ) );

                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Loot rate: " + Context.Server.Config.GameplayLootRate) );

                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Magic rate: " + Context.Server.Config.GameplayMagicLevelRate.ToString(CultureInfo.InvariantCulture) ) );

                return Promise.Completed;
            }

            return next();
        }
    }
}