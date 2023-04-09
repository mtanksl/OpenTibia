using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseTradeWithFromTileCommand : ParseTradeWithCommand
    {
        public ParseTradeWithFromTileCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId, uint creatureId) : base(player)
        {
            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;

            ToCreatureId = creatureId;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

        public uint ToCreatureId { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                Tile fromTile = context.Server.Map.GetTile(FromPosition);

                if (fromTile != null)
                {
                    if (Player.Tile.Position.CanSee(fromTile.Position) )
                    {
                        Item fromItem = fromTile.GetContent(FromIndex) as Item;

                        if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                        {
                            Player toPlayer = context.Server.GameObjects.GetPlayer(ToCreatureId);

                            if (toPlayer != null && toPlayer != Player)
                            {
                                resolve(context);
                            }
                        }
                    }
                } 
            } );     
        }
    }
}