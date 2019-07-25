using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TileReplaceItemCommand : Command
    {
        public TileReplaceItemCommand(Tile tile, byte index, ushort openTibiaId)
        {
            Tile = tile;

            Index = index;

            OpenTibiaId = openTibiaId;
        }

        public Tile Tile { get; set; }

        public byte Index { get; set; }

        public ushort OpenTibiaId { get; set; }
        
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Item item = server.ItemFactory.Create(OpenTibiaId);

            if (item != null)
            {
                //Act

                Tile.ReplaceContent(Index, item);

                //Notify

                foreach (var observer in server.Map.GetPlayers() )
                {
                    if (observer.Tile.Position.CanSee(Tile.Position) )
                    {
                        context.Write(observer.Client.Connection, new ThingUpdateOutgoingPacket(Tile.Position, Index, item) );
                    }
                }

                base.Execute(server, context);
            }            
        }
    }
}