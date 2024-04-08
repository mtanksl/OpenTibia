using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseTradeWithFromTileCommand : ParseTradeWithCommand
    {
        public ParseTradeWithFromTileCommand(Player player, Position fromPosition, byte fromIndex, ushort tibiaId, uint creatureId) : base(player)
        {
            FromPosition = fromPosition;

            FromIndex = fromIndex;

            TibiaId = tibiaId;

            ToCreatureId = creatureId;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort TibiaId { get; set; }

        public uint ToCreatureId { get; set; }

        public override Promise Execute()
        {
            Tile fromTile = Context.Server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                if (Player.Tile.Position.CanSee(fromTile.Position) )
                {
                    Item fromItem = Player.Client.GetContent(fromTile, FromIndex) as Item;

                    if (fromItem != null && fromItem.Metadata.TibiaId == TibiaId)
                    {
                        Player toPlayer = Context.Server.GameObjects.GetPlayer(ToCreatureId);

                        if (toPlayer != null && toPlayer != Player)
                        {
                            if ( IsPickupable(fromItem) )
                            {
                                return Context.AddCommand(new PlayerTradeWithCommand(this, Player, fromItem, toPlayer) );
                            }
                        }
                    }
                }
            }

            return Promise.Break;
        }
    }
}