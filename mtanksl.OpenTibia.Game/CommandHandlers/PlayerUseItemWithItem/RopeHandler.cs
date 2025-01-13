using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RopeHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> ropes;
        private readonly HashSet<ushort> ropeSpots;
        private readonly HashSet<ushort> holes;

        public RopeHandler()
        {
            ropes = Context.Server.Values.GetUInt16HashSet("values.items.ropes");
            ropeSpots = Context.Server.Values.GetUInt16HashSet("values.items.ropeSpots");
            holes = Context.Server.Values.GetUInt16HashSet("values.items.holes");
        }

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

                    Creature creature = down.TopCreature;

                    if (creature != null)
                    {
                        return Context.AddCommand(new CreatureMoveCommand(creature, south) ).Then( () =>
                        {
                            return Context.AddCommand(new CreatureUpdateDirectionCommand(creature, Direction.South) );
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