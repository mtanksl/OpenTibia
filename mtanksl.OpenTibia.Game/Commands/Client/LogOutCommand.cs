using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class LogOutCommand : Command
    {
        public LogOutCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            Player.Client.Connection.Disconnect();

            //Arrange

            Tile fromTile = Player.Tile;

            //Act

            server.Map.RemoveCreature(Player);

            byte fromIndex = fromTile.RemoveContent(Player);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer != Player)
                {
                    if (observer.Tile.Position.CanSee(fromTile.Position) )
                    {
                        context.Write(observer.Client.Connection, new ThingRemove(fromTile.Position, fromIndex) )

                              .Write(observer.Client.Connection, new ShowMagicEffect(fromTile.Position, MagicEffectType.Puff) );
                    }
                }
            }
        }
    }
}