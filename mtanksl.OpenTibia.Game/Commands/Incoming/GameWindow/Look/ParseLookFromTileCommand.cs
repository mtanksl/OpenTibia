using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseLookFromTileCommand : ParseLookCommand
    {
        public ParseLookFromTileCommand(Player player, Position fromPosition, byte fromIndex, ushort tibiaId) : base(player)
        {
            FromPosition = fromPosition;

            FromIndex = fromIndex;

            TibiaId = tibiaId;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort TibiaId { get; set; }

        public override Promise Execute()
        {
            Tile fromTile = Context.Server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                if (Player.Tile.Position.CanSee(fromTile.Position) )
                {
                    switch (Player.Client.GetContent(fromTile, FromIndex) )
                    {
                        case Item item:

                            if (item.Metadata.TibiaId == TibiaId)
                            {
                                return Context.AddCommand(new PlayerLookItemCommand(Player, item) );
                            }

                            break;

                        case Creature creature:

                            if (TibiaId == 99)
                            {
                                return Context.AddCommand(new PlayerLookCreatureCommand(Player, creature) );
                            }

                            break;
                    }
                }
            }

            return Promise.Break;
        }
    }
}