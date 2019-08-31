using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class TradeWithFromTileCommand : TradeWithCommand
    {
        public TradeWithFromTileCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId, uint creatureId) : base(player)
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

        public override void Execute(Server server, Context context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Player toPlayer = server.Map.GetCreature(ToCreatureId) as Player;

                    if (toPlayer != null && toPlayer != Player)
                    {
                        //Act

                        if ( !Player.Tile.Position.IsNextTo(fromTile.Position) )
                        {
                            WalkToUnknownPathCommand walkToUnknownPathCommand = new WalkToUnknownPathCommand(Player, fromTile);

                            walkToUnknownPathCommand.Completed += (s, e) =>
                            {
                                server.QueueForExecution(Constants.PlayerActionSchedulerEvent(Player), Constants.PlayerSchedulerEventDelay, this);
                            };

                            walkToUnknownPathCommand.Execute(server, context);
                        }
                        else
                        {
                            TradeWith(fromItem, toPlayer, server, context);
                        }
                    }
                }
            }
        }
    }
}