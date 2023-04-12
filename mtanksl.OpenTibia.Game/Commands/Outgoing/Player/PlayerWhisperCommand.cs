using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
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
            foreach (var observer in Context.Server.GameObjects.GetPlayers() )
            {
                if (observer.Tile.Position.CanHearWhisper(Player.Tile.Position) )
                {
                    Context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Whisper, Player.Tile.Position, Message) );
                }
                else if (observer.Tile.Position.CanHearSay(Player.Tile.Position) )
                {
                    Context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Whisper, Player.Tile.Position, "pspsps") );
                }
            }

            return Promise.Completed;
        }
    }
}