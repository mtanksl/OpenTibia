using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class LookFromTileCommand : LookCommand
    {
        public LookFromTileCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                IContent content = fromTile.GetContent(FromIndex);

                switch (content)
                {
                    case Item fromItem:

                        if (fromItem.Metadata.TibiaId == ItemId)
                        {
                            //Act

                            Look(Player, fromItem, server, context);

                            base.Execute(server, context);
                        }

                        break;

                    case Creature fromCreature:

                        if (ItemId == 99)
                        {
                            //Act

                            Look(Player, fromCreature, server, context);

                            base.Execute(server, context);
                        }

                        break;
                }
            }
        }
    }
}