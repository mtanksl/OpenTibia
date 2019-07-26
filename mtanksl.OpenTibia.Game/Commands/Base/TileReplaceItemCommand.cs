using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TileReplaceItemCommand : Command
    {
        public TileReplaceItemCommand(Tile tile, byte index, Item item)
        {
            Tile = tile;

            Index = index;

            Item = item;
        }

        public Tile Tile { get; set; }

        public byte Index { get; set; }

        public Item Item { get; set; }
        
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act

            Tile.ReplaceContent(Index, Item);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(Tile.Position) )
                {
                    context.Write(observer.Client.Connection, new ThingUpdateOutgoingPacket(Tile.Position, Index, Item) );
                }
            }

            base.Execute(server, context);
        }
    }
}