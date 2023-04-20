using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RopeHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> ropes = new HashSet<ushort>() { 2120, 10513, 10515, 10511 };

        private HashSet<ushort> ropeSpots = new HashSet<ushort> { 384, 418 };

        private HashSet<ushort> holes = new HashSet<ushort>() 
        { 
            // Pitfall
            294,

            // Hole
            383, 469, 470, 482, 484, 485, 369, 409, 410, 411, 

            // Invisible
            459, 

            // Wooden
            369, 409, 410, 411,

            // Wooden
            423, 427, 428, 429,

            // White marble
            432, 433,

            // Dark wooden
            3135, 3136, 3137, 3138,

            // Stone
            3219, 3220,

            // Black marble
            4834, 4835, 

            // Gray marble
            4836, 4837,

            // Stone
            475, 476, 479, 480, 6127, 6128, 6129, 6130,

            // Snow
            6917, 6918, 6919, 6920, 6921, 6922, 6923, 6924,

            // Earth
            8559, 8560, 8561, 8562, 8563, 8564, 8565, 8566
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (ropes.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (ropeSpots.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    Tile up = Context.Server.Map.GetTile( ( (Tile)command.ToItem.Parent ).Position.Offset(0, 1, -1) );

                    return Context.AddCommand(new CreatureUpdateTileCommand(command.Player, up, Direction.South) );
                }
                else if (holes.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    Tile down = Context.Server.Map.GetTile( ( (Tile)command.ToItem.Parent).Position.Offset(0, 0, 1) );

                    Tile south = Context.Server.Map.GetTile( ( (Tile)command.ToItem.Parent ).Position.Offset(0, 1, 0) );

                    if (down.TopCreature != null)
                    {
                        Creature creature = down.TopCreature;

                        return Context.AddCommand(new CreatureUpdateTileCommand(creature, south, Direction.South) );
                    }
                    else if (down.TopItem != null)
                    {
                        Item item = down.TopItem;

                        return Context.AddCommand(new TileRemoveItemCommand(down, item) ).Then( () =>
                        {
                            return Context.AddCommand(new TileAddItemCommand(south, item) );
                        } );
                    }
                }
            }

            return next();
        }
    }
}