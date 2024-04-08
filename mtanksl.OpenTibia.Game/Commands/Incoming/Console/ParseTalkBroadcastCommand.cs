using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseTalkBroadcastCommand : IncomingCommand
    {
        public ParseTalkBroadcastCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            // #b <message>

            if (Player.Rank == Rank.Gamemaster)
            {
                ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, TalkType.Broadcast, Message);

                foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                {
                    Context.AddPacket(observer, showTextOutgoingPacket);
                }

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}