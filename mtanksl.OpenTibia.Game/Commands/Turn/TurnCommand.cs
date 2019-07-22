using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TurnCommand : Command
    {
        public TurnCommand(Player player, Direction direction)
        {
            Player = player;

            Direction = direction;
        }

        public Player Player { get; set; }

        public Direction Direction { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = Player.Tile;

            byte fromIndex = fromTile.GetIndex(Player);

            if (Player.Direction != Direction)
            {
                //Act

                Player.Direction = Direction;

                //Notify

                foreach (var observer in server.Map.GetPlayers() )
                {
                    if (observer.Tile.Position.CanSee(fromTile.Position) )
                    {
                        context.Write(observer.Client.Connection, new ThingUpdateOutgoingPacket(fromTile.Position, fromIndex, Player.Id, Direction) );                        
                    }
                }

                base.Execute(server, context);
            }
        }
    }
}