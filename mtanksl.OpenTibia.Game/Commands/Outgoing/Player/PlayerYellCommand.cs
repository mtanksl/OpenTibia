using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

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
            foreach (var observer in Context.Server.Map.GetObservers(Player.Tile.Position).OfType<Player>() )
            {
                if (observer.Tile.Position.CanHearYell(Player.Tile.Position) )
                {
                    Context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Yell, Player.Tile.Position, Message.ToUpper() ) );
                 
                    Context.AddEvent(observer, new PlayerYellEventArgs(Player, Message) );
                }
            }

            Context.AddEvent(new PlayerYellEventArgs(Player, Message) );

            return Promise.Completed;
        }
    }
}