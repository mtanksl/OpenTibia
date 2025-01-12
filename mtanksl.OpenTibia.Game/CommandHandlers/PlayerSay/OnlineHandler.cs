using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class OnlineHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("!online") ) 
            {
                var players = Context.Server.GameObjects.GetPlayers().ToArray();

                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.PurpleDefault, "Players online: " + players.Length) );

                for (int i = 0; i < players.Length; i += 10)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.PurpleDefault, string.Join(", ", players.Skip(i).Take(10).Select(p => p.Name + " (" + p.Level + ")") ) ) );
                }

                return Promise.Completed;
            }

            return next();
        }
    }
}