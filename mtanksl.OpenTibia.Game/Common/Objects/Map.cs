using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Otbm;
using OpenTibia.Game;
using System.Collections.Generic;
using OtbmItem = OpenTibia.FileFormats.Otbm.Item;

namespace OpenTibia.Common.Objects
{
    public class Map : IMap
    {
        private class PositionEqualityComparer : IEqualityComparer<Position>
        {
            private int x;

            private int y;

            private int width;

            private int height;

            public PositionEqualityComparer(int minX, int minY, int maxX, int maxY)
            {
                this.x = minX;

                this.y = minY;

                this.width = maxX - minX;

                this.height = maxY - minY;
            }

            public bool Equals(Position objA, Position objB)
            {
                return objA.Equals(objB);
            }

            public int GetHashCode(Position obj)
            {
                if (width < 16384 && height < 16384)
                {
                    return (obj.X - x) << 18 | (obj.Y - y) << 4 | obj.Z;
                }

                return obj.GetHashCode();
            }
        }

        public Map(ItemFactory itemFactory, OtbmFile otbmFile)
        {
            if (otbmFile.Towns != null)
            {
                this.towns = new Dictionary<string, Town>(otbmFile.Towns.Count);

                foreach (var town in otbmFile.Towns)
                {
                    towns.Add(town.Name, new Town() 
                    { 
                        Id = town.Id,

                        Name = town.Name,

                        Position = town.Position
                    } );
                }
            }
            else
            {
                this.towns = new Dictionary<string, Town>();
            }

            if (otbmFile.Waypoints != null)
            {
                this.waypoints = new Dictionary<string, Waypoint>(otbmFile.Waypoints.Count);

                foreach (var waypoint in otbmFile.Waypoints)
                {
                    waypoints.Add(waypoint.Name, new Waypoint()
                    {
                        Name = waypoint.Name,

                        Position = waypoint.Position
                    } );
                }
            }
            else
            {
                this.waypoints = new Dictionary<string, Waypoint>();
            }

            int count = 0;

            int minX = int.MaxValue;

            int minY = int.MaxValue;

            int maxX = 0;
            
            int maxY = 0;

            foreach (var otbmArea in otbmFile.Areas)
            {
                foreach (var otbmTile in otbmArea.Tiles)
                {
                    count++;
                }

                if (otbmArea.Position.X < minX)
                {
                    minX = otbmArea.Position.X;
                }

                if (otbmArea.Position.Y < minY)
                {
                    minY = otbmArea.Position.Y;
                }

                if (otbmArea.Position.X > maxX)
                {
                    maxX = otbmArea.Position.X;
                }

                if (otbmArea.Position.Y > maxY)
                {
                    maxY = otbmArea.Position.Y;
                }
            }

            this.tiles = new Dictionary<Position, Tile>(count, new PositionEqualityComparer(minX, minY, maxX + 256, maxY + 256) );

            foreach (var otbmArea in otbmFile.Areas)
            {
                foreach (var otbmTile in otbmArea.Tiles)
                {
                    Tile tile = new Tile(otbmArea.Position.Offset(otbmTile.OffsetX, otbmTile.OffsetY, 0) );

                    tiles.Add(tile.Position, tile);

                    if (otbmTile.OpenTibiaItemId > 0)
                    {
                        Item ground = itemFactory.Create(otbmTile.OpenTibiaItemId, 1);
                        
                        tile.AddContent(ground);
                    }

                    if (otbmTile.Items != null)
                    {
                        void AddItems(IContainer parent, List<OtbmItem> items)
                        {
                            foreach (var otbmItem in items)
                            {
                                Item item = itemFactory.Create(otbmItem.OpenTibiaId, otbmItem.Count);

                                switch (item)
                                {
                                    case TeleportItem teleport:

                                        teleport.Position = otbmItem.TeleportPosition;

                                        break;

                                    case Locker locker:

                                        locker.TownId = otbmItem.DepotId;

                                        break;

                                    case Container container:

                                        if (otbmItem.Items != null)
                                        {
                                            AddItems(container, otbmItem.Items);
                                        }

                                        break;

                                    case ReadableItem readableItem:

                                        readableItem.Text = otbmItem.Text;

                                        readableItem.Author = otbmItem.WrittenBy;

                                        break;
                                }

                                parent.AddContent(item);
                            }
                        }

                        AddItems(tile, otbmTile.Items);
                    }
                }
            }
        }

        private Dictionary<string, Town> towns;

        public Town GetTown(string name)
        {
            Town town;

            towns.TryGetValue(name, out town);

            return town;
        }

        public IEnumerable<Town> GetTowns()
        {
            return towns.Values;
        }

        private Dictionary<string, Waypoint> waypoints;

        public Waypoint GetWaypoint(string name)
        {
            Waypoint waypoint;

            waypoints.TryGetValue(name, out waypoint);

            return waypoint;
        }

        public IEnumerable<Waypoint> GetWaypoints()
        {
            return waypoints.Values;
        }

        private Dictionary<Position, Tile> tiles;

        public Tile GetTile(Position position)
        {
            Tile tile;

            tiles.TryGetValue(position, out tile);

            return tile;
        }

        public IEnumerable<Tile> GetTiles()
        {
            return tiles.Values;
        }
    }
}