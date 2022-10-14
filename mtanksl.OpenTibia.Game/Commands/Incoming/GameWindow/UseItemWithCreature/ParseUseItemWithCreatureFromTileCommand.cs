using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithCreatureFromTileCommand : ParseUseItemWithCreatureCommand
    {
        public ParseUseItemWithCreatureFromTileCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId, uint toCreatureId) : base(player)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;

            ToCreatureId = toCreatureId;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

        public uint ToCreatureId { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Tile fromTile = context.Server.Map.GetTile(FromPosition);

                if (fromTile != null)
                {
                    Item fromItem = fromTile.GetContent(FromIndex) as Item;

                    if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                    {
                        Creature toCreature = context.Server.GameObjects.GetCreature(ToCreatureId);

                        if (toCreature != null)
                        {
                            if ( IsUseable(context, fromItem) )
                            {
                                context.AddCommand(new PlayerUseItemWithCreatureCommand(Player, fromItem, toCreature) ).Then(ctx =>
                                {
                                    resolve(ctx);
                                } );
                            }
                        }
                    }
                }
            } );
        }
    }
}