using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class StairsScript : ICreatureWalkScript, IItemMoveScript
    {
        private HashSet<ushort> useable = new HashSet<ushort>() { 468, 481, 483 };

        public void Register(Server server)
        {
            foreach (var tile in server.Map.GetTiles() )
            {
                FloorChange floorChange = tile.FloorChange;

                if (floorChange == FloorChange.None)
                {
                    foreach (var item in tile.GetItems() )
                    {
                        if (useable.Contains(item.Metadata.OpenTibiaId) )
                        {
                            positions.Add(tile.Position, floorChange);
                        }

                        break;
                    }
                }
                else
                {
                    positions.Add(tile.Position, floorChange);
                }
            }

            server.CreatureWalkScripts.Add(this);

            server.ItemMoveScripts.Add(this);
        }

        private Dictionary<Position, FloorChange> positions = new Dictionary<Position, FloorChange>();
        
        public bool Execute(Creature creature, Tile fromTile, Tile toTile, Server server, CommandContext context)
        {
            FloorChange floorChange;

            if (positions.TryGetValue(toTile.Position, out floorChange) )
            {
                if (floorChange == FloorChange.None)
                {
                    floorChange = toTile.FloorChange;

                    if (floorChange != FloorChange.None)
                    {
                        Tile nextTile = server.Map.GetTile( GetPosition(toTile.Position, floorChange) );

                        if (nextTile != null)
                        {
                            toTile = nextTile;
                        }

                        new CreatureMoveCommand(creature, toTile).Execute(server, context);

                        return true;
                    }
                }
                else
                {
                    Tile nextTile = server.Map.GetTile( GetPosition(toTile.Position, floorChange) );

                    if (nextTile != null)
                    {
                        toTile = nextTile;
                    }

                    new CreatureMoveCommand(creature, toTile).Execute(server, context);

                    return true;
                }
            }

            return false;
        }

        public bool Execute(Player player, Item fromItem, IContainer toContainer, byte toIndex, byte count, Server server, CommandContext context)
        {
            if (toContainer is Tile toTile)
            {
                FloorChange floorChange;

                if (positions.TryGetValue(toTile.Position, out floorChange) )
                {
                    if (floorChange == FloorChange.None)
                    {
                        floorChange = toTile.FloorChange;

                        if (floorChange != FloorChange.None)
                        {
                            Tile nextTile = server.Map.GetTile( GetPosition(toTile.Position, floorChange) );

                            if (nextTile != null)
                            {
                                toTile = nextTile;
                            }

                            new ItemMoveCommand(player, fromItem, toTile, 0, count).Execute(server, context);

                            return true;
                        }
                    }
                    else
                    {
                        Tile nextTile = server.Map.GetTile( GetPosition(toTile.Position, floorChange) );

                        if (nextTile != null)
                        {
                            toTile = nextTile;
                        }

                        new ItemMoveCommand(player, fromItem, toTile, 0, count).Execute(server, context);

                        return true;
                    }
                }
            }

            return false;
        }

        private Position GetPosition(Position toPosition, FloorChange floorChange)
        {
            switch (floorChange)
            {
                case FloorChange.Down:

                    toPosition = toPosition.Offset(0, 0, 1);

                    if (positions.TryGetValue(toPosition, out floorChange) )
                    {
                        switch (floorChange)
                        {
                            case FloorChange.East:

                                toPosition = toPosition.Offset(-1, 0, 0);

                                break;

                            case FloorChange.North:

                                toPosition = toPosition.Offset(0, 1, 0);

                                break;

                            case FloorChange.West:

                                toPosition = toPosition.Offset(1, 0, 0);

                                break;

                            case FloorChange.South:

                                toPosition = toPosition.Offset(0, -1, 0);

                                break;

                            case FloorChange.NorthEast:

                                toPosition = toPosition.Offset(-1, 1, 0);

                                break;

                            case FloorChange.NorthWest:

                                toPosition = toPosition.Offset(1, 1, 0);

                                break;

                            case FloorChange.SouthEast:

                                toPosition = toPosition.Offset(-1, -1, 0);

                                break;

                            case FloorChange.SouthWest:

                                toPosition = toPosition.Offset(1, -1, 0);

                                break;
                        }
                    }

                    break;

                case FloorChange.East:

                    toPosition = toPosition.Offset(1, 0, -1);

                    break;

                case FloorChange.North:

                    toPosition = toPosition.Offset(0, -1, -1);

                    break;

                case FloorChange.West:

                    toPosition = toPosition.Offset(-1, 0, -1);

                    break;

                case FloorChange.South:

                    toPosition = toPosition.Offset(0, 1, -1);

                    break;

                case FloorChange.NorthEast:

                    toPosition = toPosition.Offset(1, -1, -1);

                    break;

                case FloorChange.NorthWest:

                    toPosition = toPosition.Offset(-1, -1, -1);

                    break;

                case FloorChange.SouthEast:

                    toPosition = toPosition.Offset(1, 1, -1);

                    break;

                case FloorChange.SouthWest:

                    toPosition = toPosition.Offset(-1, 1, -1);

                    break;
            }

            return toPosition;
        }
    }
}