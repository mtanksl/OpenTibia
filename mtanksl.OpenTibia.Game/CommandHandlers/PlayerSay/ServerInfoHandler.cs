using OpenTibia.Common.Structures;
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
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.Say, command.Message) );

                if (Context.Server.Config.GameplayWorldType == WorldType.Pvp)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "World type: pvp") );

                    if (Context.Server.Config.GameplayProtectionLevel > 0)
                    {
                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Protection level: " + Context.Server.Config.GameplayProtectionLevel) );
                    }
                }
                else
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "World type: non-pvp") );
                }
         
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Experience rate: " + Context.Server.Config.GameplayExperienceRate.ToString(CultureInfo.InvariantCulture) ) );

                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Skill rate: " + Context.Server.Config.GameplaySkillRate.ToString(CultureInfo.InvariantCulture) ) );

                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Loot rate: " + Context.Server.Config.GameplayLootRate) );

                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Magic rate: " + Context.Server.Config.GameplayMagicLevelRate.ToString(CultureInfo.InvariantCulture) ) );

                return Promise.Completed;
            }

            return next();
        }
    }
}