using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseLookFromTileCommand : ParseLookCommand
    {
        public ParseLookFromTileCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId) : base(player)
        {
            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                Tile fromTile = Context.Server.Map.GetTile(FromPosition);

                if (fromTile != null)
                {
                    if (Player.Tile.Position.CanSee(fromTile.Position) )
                    {
                        switch ( fromTile.GetContent(FromIndex) )
                        {
                            case Item item:

                                if (item.Metadata.TibiaId == ItemId)
                                {
                                    Context.AddCommand(new PlayerLookItemCommand(Player, item) ).Then( () =>
                                    {
                                        resolve();
                                    } );
                                }

                                break;

                            case Creature creature:

                                if (ItemId == 99)
                                {
                                    Context.AddCommand(new PlayerLookCreatureCommand(Player, creature) ).Then( () =>
                                    {
                                        resolve();
                                    } );
                                }

                                break;
                        }
                    }
                }
            } );            
        }
    }
}