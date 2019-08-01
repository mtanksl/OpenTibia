using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TileAddItemCommand : Command
    {
        public TileAddItemCommand(Tile tile, Item item)
        {
            Tile = tile;

            Item = item;
        }

        public Tile Tile { get; set; }

        public Item Item { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act

            byte index = Tile.AddContent(Item);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(Tile.Position) )
                {
                    context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(Tile.Position, index, Item) );
                }
            }

            foreach (var script in server.Scripts.TileAddItemScripts)
            {
                script.OnTileAddItem(Item, Tile, index, server, context);
            }

            base.Execute(server, context);
        }
    }
}