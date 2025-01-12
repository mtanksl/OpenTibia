using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
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

                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Experience rate: " + Context.Server.Config.GameplayExperienceRate.ToString(CultureInfo.InvariantCulture) ) );

                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Skill rate: 1") );

                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Loot rate: " + Context.Server.Config.GameplayLootRate) );

                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Magic rate: 1") );

                return Promise.Completed;
            }

            return next();
        }
    }
}