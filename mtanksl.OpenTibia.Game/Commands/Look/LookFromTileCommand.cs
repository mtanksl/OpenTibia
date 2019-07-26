using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class LookFromTileCommand : Command
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
                Command command = null;

                switch ( fromTile.GetContent(FromIndex) )
                {
                    case Item item:

                        if (item.Metadata.TibiaId == ItemId)
                        {
                            command = new LookItemCommand(Player, item);
                        }

                        break;

                    case Creature creature:

                        if (ItemId == 99)
                        {
                            command = new LookCreatureCommand(Player, creature);
                        }

                        break;
                }

                if (command != null)
                {
                    command.Completed += (s, e) =>
                    {
                        //Act

                        base.Execute(server, context);
                    };

                    command.Execute(server, context);
                }
            }
        }
    }
}