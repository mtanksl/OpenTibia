using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

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
            foreach (var observer in Context.Server.Map.GetObservers(Player.Tile.Position).OfType<Player>() )
            {
                if (observer.Tile.Position.CanHearWhisper(Player.Tile.Position) )
                {
                    Context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Whisper, Player.Tile.Position, Message) );

                    Context.AddEvent(observer, new PlayerWhisperEventArgs(Player, Message) );
                }
                else if (observer.Tile.Position.CanHearSay(Player.Tile.Position) )
                {
                    Context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Whisper, Player.Tile.Position, "pspsps") );

                    Context.AddEvent(observer, new PlayerWhisperEventArgs(Player, Message) );
                }
            }

            Context.AddEvent(new PlayerWhisperEventArgs(Player, Message) );

            return Promise.Completed;
        }
    }
}