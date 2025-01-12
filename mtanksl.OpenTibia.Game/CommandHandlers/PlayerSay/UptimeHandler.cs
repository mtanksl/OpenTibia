using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class UptimeHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("!uptime") ) 
            {
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.PurpleDefault, command.Message) );

                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Uptime: " + Context.Server.Statistics.Uptime.Days + " days " + Context.Server.Statistics.Uptime.Hours + " hours " + Context.Server.Statistics.Uptime.Minutes + " minutes") );

                return Promise.Completed;
            }

            return next();
        }
    }
}