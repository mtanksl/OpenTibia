﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RopeHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private static HashSet<ushort> ropes = new HashSet<ushort>() { 2120, 10513, 10515, 10511 };

        private static HashSet<ushort> ropeSpots = new HashSet<ushort> { 384, 418, 8278 };

        private static HashSet<ushort> holes = new HashSet<ushort>()
        { 
            // Pitfall
            294,

            // Hole
            383, 469, 470, 482, 484, 485, 

            // Invisible
            459, 

            // Wooden
            369, 370, 408, 409, 410, 411, 8276, 8277, 8279, 8280, 8281, 8282, 
            
            // Wooden
            423, 427, 428, 429, 8283, 8284, 8285, 8286, 
                        
            // White marble
            432, 433, 9606,

            // Dark wooden
            3135, 3136, 3137, 3138,
                       
            // Stone
            3219, 3220,

            // Black marble
            4834, 4835, 8170,

            // Gray marble
            4836, 4837,

            // Flat roof
            924, 5081,

            // Grass roof
            6173, 6174,
                      
            // Stone
            475, 476, 479, 480, 6127, 6128, 6129, 6130,

            // Snow
            6917, 6918, 6919, 6920, 6921, 6922, 6923, 6924,

            // Earth
            8559, 8560, 8561, 8562, 8563, 8564, 8565, 8566,

            // Corkscrew
            9574, 9846
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (ropes.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (ropeSpots.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    Tile ropeSpot = (Tile)command.ToItem.Parent;

                    Tile up = Context.Server.Map.GetTile(ropeSpot.Position.Offset(0, 1, -1) );

                    return Context.AddCommand(new CreatureMoveCommand(command.Player, up) ).Then( () =>
                    {
                        return Context.AddCommand(new CreatureUpdateDirectionCommand(command.Player, Direction.South) );
                    } );
                }
                else if (holes.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    Tile hole = (Tile)command.ToItem.Parent;

                    Tile down = Context.Server.Map.GetTile(hole.Position.Offset(0, 0, 1) );

                    Tile south = Context.Server.Map.GetTile(hole.Position.Offset(0, 1, 0) );

                    if (down.TopCreature != null)
                    {
                        return Context.AddCommand(new CreatureMoveCommand(down.TopCreature, south) ).Then( () =>
                        {
                            return Context.AddCommand(new CreatureUpdateDirectionCommand(down.TopCreature, Direction.South) );
                        } );
                    }
                    else if (down.TopItem != null && !down.TopItem.Metadata.Flags.Is(ItemMetadataFlags.NotMoveable) )
                    {
                        return Context.AddCommand(new ItemMoveCommand(down.TopItem, south, 0) );
                    }
                }
            }

            return next();
        }
    }
}