using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class HelpHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("!help") ) 
            {
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.PurpleDefault, command.Message) );

                List<string> commands = new List<string>()
                {
                    "!help",
                    "!online",
                    "!serverinfo",
                    "!uptime",
                };

                if (Context.Server.Config.Rules != null)
                {
                    commands.Add("!rules");
                }

                if (command.Player.Rank == Rank.Gamemaster)
                {
                    commands.Add("!commands");
                }

                string message = "Available commands: " + string.Join(", ", commands);

                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, message) );

                return Promise.Completed;
            }

            return next();
        }
    }
}