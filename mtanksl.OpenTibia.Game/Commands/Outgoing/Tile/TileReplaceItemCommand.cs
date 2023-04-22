using OpenTibia.Common.Objects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TileReplaceItemCommand : Command
    {
        public TileReplaceItemCommand(Tile tile, Item fromItem, Item toItem)
        {
            Tile = tile;

            FromItem = fromItem;

            ToItem = toItem;
        }

        public Tile Tile { get; set; }

        public Item FromItem { get; set; }

        public Item ToItem { get; set; }
        
        public override Promise Execute()
        {
            byte index = Tile.GetIndex(FromItem);

            Tile.ReplaceContent(index, ToItem);

            foreach (var observer in Context.Server.GameObjects.GetPlayers() )
            {
                byte clientIndex;

                if (observer.Client.TryGetIndex(ToItem, out clientIndex) )
                {
                    Context.AddPacket(observer.Client.Connection, new ThingUpdateOutgoingPacket(Tile.Position, clientIndex, ToItem) );
                }
            }            

            Context.AddEvent(new TileReplaceItemEventArgs(Tile, FromItem, ToItem, index) );

            return Promise.Completed;
        }
    }
}