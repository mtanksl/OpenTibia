using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
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
            return Promise.Run( (resolve, reject) =>
            {
                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    if (observer.Tile.Position.CanHearYell(Player.Tile.Position) )
                    {
                        context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Yell, Player.Tile.Position, Message.ToUpper() ) );
                    }
                }

                resolve(context);
            } );
        }
    }
}