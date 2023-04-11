using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
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
            return Promise.Run( (resolve, reject) =>
            {
                foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                {
                    if (observer.Tile.Position.CanHearSay(Player.Tile.Position) )
                    {
                        Context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Say, Player.Tile.Position, Message) );
                    }
                }

                resolve();
            } );
        }
    }
}