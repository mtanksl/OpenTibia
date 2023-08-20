using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Otbm;
using OpenTibia.FileFormats.Xml.Houses;
using OpenTibia.FileFormats.Xml.Spawns;
using OpenTibia.Game;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private int minX = int.MaxValue;

        private int minY = int.MaxValue;

        private int maxX = 0;

        private int maxY = 0;

        public Map(ItemFactory itemFactory, OtbmFile otbmFile, SpawnFile spawnFile, HouseFile houseFile)
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

            foreach (var otbmArea in otbmFile.Areas)
            {
                foreach (var otbmTile in otbmArea.Tiles)
                {
                    count++;

                    if (otbmArea.Position.X + otbmTile.OffsetX < minX)
                    {
                        minX = otbmArea.Position.X + otbmTile.OffsetX;
                    }

                    if (otbmArea.Position.Y + otbmTile.OffsetY < minY)
                    {
                        minY = otbmArea.Position.Y + otbmTile.OffsetY;
                    }

                    if (otbmArea.Position.X + otbmTile.OffsetX > maxX)
                    {
                        maxX = otbmArea.Position.X + otbmTile.OffsetX;
                    }

                    if (otbmArea.Position.Y + otbmTile.OffsetY > maxY)
                    {
                        maxY = otbmArea.Position.Y + otbmTile.OffsetY;
                    }
                }
            }

            this.tiles = new Dictionary<Position, Tile>(count, new PositionEqualityComparer(minX, minY, maxX, maxY) );

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

                                item.ActionId = otbmItem.ActionId;

                                item.UniqueId = otbmItem.UniqueId;

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

            observers = new HashSet<Creature>[ (int)Math.Ceiling( (maxY - minY) / 13.0) ][];

            for (int j = 0; j < observers.Length; j++)
            {
                observers[j] = new HashSet<Creature>[ (int)Math.Ceiling( (maxX - minX) / 17.0) ];

                for (int i = 0; i < observers[j].Length; i++)
                {
                    observers[j][i] = null;
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

        //TODO: Implement Quadtree

        private HashSet<Creature>[][] observers;

        public void AddObserver(Position position, Creature creature)
        {
            int j = (position.Y - minY) / 13;

            int i = (position.X - minX) / 17;

            HashSet<Creature> set = observers[j][i];

            if (set == null)
            {
                set = new HashSet<Creature>();

                observers[j][i] = set;
            }

            set.Add(creature);
        }

        public void RemoveObserver(Position position, Creature creature)
        {
            int j = (position.Y - minY) / 13;

            int i = (position.X - minX) / 17;

            HashSet<Creature> set = observers[j][i];

            if (set != null)
            {
                set.Remove(creature);

                if (set.Count == 0)
                {
                    observers[j][i] = null;
                }
            }
        }

        public IEnumerable<Creature> GetObserversOfTypeCreature(Position position)
        {
            int Div(int a, int b)
            {
                if (a >= 0)
                {
                    return a / b;
                }

                return (a - b + 1) / b;
            }

            HashSet<Creature> GetObservers(int positionX, int positionY)
            {
                int j = Div(positionY - minY, 13);

                if (j < 0 || j > observers.Length)
                {
                    return null;
                }

                int i = Div(positionX - minX, 17);

                if (i < 0 || i > observers[0].Length)
                {
                    return null;
                }

                return observers[j][i];
            }

            HashSet<Creature> creatures = new HashSet<Creature>();

            HashSet<Creature> topLeft = GetObservers(position.X - 8, position.Y - 6);

            if (topLeft != null)
            {
                creatures.UnionWith(topLeft);
            }

            HashSet<Creature> topRight = GetObservers(position.X + 9, position.Y - 6);

            if (topRight != null)
            {
                creatures.UnionWith(topRight);
            }

            HashSet<Creature> bottomLeft = GetObservers(position.X - 8, position.Y + 7);

            if (bottomLeft != null)
            {
                creatures.UnionWith(bottomLeft);
            }

            HashSet<Creature> bottomRight = GetObservers(position.X + 9, position.Y + 7);

            if (bottomRight != null)
            {
                creatures.UnionWith(bottomRight);
            }

            return creatures;
        }

        public IEnumerable<Player> GetObserversOfTypePlayer(Position position)
        {
            return GetObserversOfTypeCreature(position).OfType<Player>();
        }

        public IEnumerable<Monster> GetObserversOfTypeMonster(Position position)
        {
            return GetObserversOfTypeCreature(position).OfType<Monster>();
        }

        public IEnumerable<Npc> GetObserversOfTypeNpc(Position position)
        {
            return GetObserversOfTypeCreature(position).OfType<Npc>();
        }
    }
}