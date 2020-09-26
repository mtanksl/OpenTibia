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

        public override void Execute(Context context)
        {
            Tile fromTile = context.Server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Player toPlayer = context.Server.GameObjects.GetGameObject<Creature>(ToCreatureId) as Player;

                    if (toPlayer != null && toPlayer != Player)
                    {
                        if ( !Player.Tile.Position.IsNextTo(fromTile.Position) )
                        {
                            WalkToUnknownPathCommand walkToUnknownPathCommand = new WalkToUnknownPathCommand(Player, fromTile);

                            walkToUnknownPathCommand.Completed += (s, e) =>
                            {
                                context.Server.QueueForExecution(Constants.CreatureActionSchedulerEvent(Player), Constants.CreatureActionSchedulerEventDelay, this);
                            };

                            walkToUnknownPathCommand.Execute(context);
                        }
                        else
                        {
                            TradeWith(fromItem, toPlayer, context);
                        }
                    }
                }
            }
        }
    }
}