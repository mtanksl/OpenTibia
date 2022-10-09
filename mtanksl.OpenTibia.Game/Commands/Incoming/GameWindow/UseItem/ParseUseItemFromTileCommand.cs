using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemFromTileCommand : ParseUseItemCommand
    {
        public ParseUseItemFromTileCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId) : base(player)
        {
            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

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
                        context.AddCommand(new PlayerUseItemCommand(Player, fromItem, null) ).Then(ctx =>
                        {
                            resolve(ctx);
                        } );
                    }
                }
            } );   
        }
    }
}