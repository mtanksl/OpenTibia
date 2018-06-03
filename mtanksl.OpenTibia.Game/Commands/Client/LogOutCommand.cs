using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class LogOutCommand : Command
    {
        private Server server;

        public LogOutCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public override void Execute(Context context)
        {
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
                        context.Response.Write(observer.Client.Connection, new ThingRemove(fromTile.Position, fromIndex) )

                            .Write(observer.Client.Connection, new ShowMagicEffect(fromTile.Position, MagicEffectType.Puff) );
                    }
                }
            }
        }
    }
}