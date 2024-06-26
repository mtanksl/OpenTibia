﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseTalkPrivateCommand : IncomingCommand
    {
        public ParseTalkPrivateCommand(Player player, string name, string message)
        {
            Player = player;

            Name = name;

            Message = message;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            // *<player>* <message>

            Player observer = Context.Server.GameObjects.GetPlayerByName(Name);

            if (observer != null && observer != Player)
            {
                Context.AddPacket(observer, new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, TalkType.Private, Message) );

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}