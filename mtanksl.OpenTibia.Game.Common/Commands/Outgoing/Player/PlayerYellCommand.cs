﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerYellCommand : Command
    {
        public PlayerYellCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, MessageMode.Yell, Player.Tile.Position, Message.ToUpper() );

            foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Player.Tile.Position) )
            {
                if (observer.Tile.Position.CanHearYell(Player.Tile.Position) )
                {
                    Context.AddPacket(observer, showTextOutgoingPacket);
                }
            }

            Context.AddEvent(new PlayerYellEventArgs(Player, Message) );

            return Promise.Completed;
        }
    }
}