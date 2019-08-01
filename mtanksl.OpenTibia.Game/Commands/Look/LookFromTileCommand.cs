using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class LookFromTileCommand : LookCommand
    {
        public LookFromTileCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId) : base(player)
        {
            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                switch ( fromTile.GetContent(FromIndex) )
                {
                    case Item item:

                        if (item.Metadata.TibiaId == ItemId)
                        {
                            //Act

                            LookItem(item, server, context);
                        }

                        break;

                    case Creature creature:

                        if (ItemId == 99)
                        {
                            //Act

                            LookCreature(creature, server, context);
                        }

                        break;
                }
            }
        }
    }
}