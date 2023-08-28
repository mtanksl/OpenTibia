using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerSayCommand : Command
    {
        public PlayerSayCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            foreach (var observer in Context.Server.Map.GetObserversOfTypeCreature(Player.Tile.Position) )
            {
                if (observer.Tile.Position.CanHearSay(Player.Tile.Position) )
                {
                    if (observer is Player player)
                    {
                        Context.AddPacket(player.Client.Connection, new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, TalkType.Say, Player.Tile.Position, Message) );
                    }
                   
                    Context.AddEvent(observer, new PlayerSayEventArgs(Player, Message) );
                }
            }

            Context.AddEvent(new PlayerSayEventArgs(Player, Message) );

            return Promise.Completed;
        }
    }
}