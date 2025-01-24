using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerWhisperCommand : Command
    {
        public PlayerWhisperCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, TalkType.Whisper, Player.Tile.Position, Message);

            ShowTextOutgoingPacket showTextOutgoingPacket2 = new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Whisper, Player.Tile.Position, "pspsps");

            PlayerWhisperEventArgs playerWhisperEventArgs = new PlayerWhisperEventArgs(Player, Message);

            foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Player.Tile.Position) )
            {
                if (observer.Tile.Position.CanHearWhisper(Player.Tile.Position) )
                {
                    Context.AddPacket(observer, showTextOutgoingPacket);
                }
                else if (observer.Tile.Position.CanHearSay(Player.Tile.Position) )
                {
                    Context.AddPacket(observer, showTextOutgoingPacket2);
                }
            }

            Context.AddEvent(playerWhisperEventArgs);

            return Promise.Completed;
        }
    }
}