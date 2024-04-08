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
                if (Player.Tile.Position.CanHearSay(fromTile.Position) )
                {
                    if (TibiaId == 99)
                    {
                        Creature creature = fromTile.TopCreature;

                        if (creature != null)
                        {
                            return Context.AddCommand(new PlayerLookCreatureCommand(this, Player, creature) );
                        }
                    }
                    else
                    {
                        switch (Player.Client.GetContent(fromTile, FromIndex) )
                        {
                            case Item item:

                                if (item.Metadata.TibiaId == TibiaId)
                                {
                                    return Context.AddCommand(new PlayerLookItemCommand(this, Player, item) );
                                }

                                break;
                        }
                    }
                }
            }

            return Promise.Break;
        }
    }
}