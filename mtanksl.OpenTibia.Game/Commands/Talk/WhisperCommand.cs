using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class WhisperCommand : Command
    {
        public WhisperCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer != Player)
                {
                    if (observer.Tile.Position.CanHearWhisper(Player.Tile.Position) )
                    {
                        context.Write(observer.Client.Connection, new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Whisper, Player.Tile.Position, Message) );
                    }
                    else if (observer.Tile.Position.CanHearSay(Player.Tile.Position) )
                    {
                        context.Write(observer.Client.Connection, new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Whisper, Player.Tile.Position, "pspsps") );
                    }
                }
            }

            context.Write(Player.Client.Connection, new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Whisper, Player.Tile.Position, Message) );
        }
    }
}