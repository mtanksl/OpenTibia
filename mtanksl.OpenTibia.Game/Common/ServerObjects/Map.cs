using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Otbm;
using OpenTibia.FileFormats.Xml.Houses;
using OpenTibia.FileFormats.Xml.Monsters;
using OpenTibia.FileFormats.Xml.Spawns;
using OpenTibia.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

                this.width = maxX - minX + 1;

                this.height = maxY - minY + 1;
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

        private Server server;

        private int minX = int.MaxValue;

        private int minY = int.MaxValue;

        private int maxX = 0;

        private int maxY = 0;

        public Map(Server server)
        {
            this.server = server;
        }

        public void Start(OtbmFile otbmFile, SpawnFile spawnFile, HouseFile houseFile)
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

                    tile.ProtectionZone = (otbmTile.Flags & TileFlags.ProtectionZone) == TileFlags.ProtectionZone;

                    tiles.Add(tile.Position, tile);

                    if (otbmTile.OpenTibiaItemId > 0)
                    {
                        Item ground = server.ItemFactory.Create(otbmTile.OpenTibiaItemId, 1);

                        server.ItemFactory.Attach(ground);

                        tile.AddContent(ground);
                    }

                    if (otbmTile.Items != null)
                    {
                        void AddItems(IContainer parent, List<OtbmItem> items)
                        {
                            foreach (var otbmItem in items)
                            {
                                Item item = server.ItemFactory.Create(otbmItem.OpenTibiaId, otbmItem.Count);

                                server.ItemFactory.Attach(item);

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

            foreach (var spawn in spawnFile.Spawns)
            {
                foreach (var xmlMonster in spawn.Monsters)
                {
                    Tile tile = GetTile(xmlMonster.Position);

                    Monster monster = server.MonsterFactory.Create(xmlMonster.Name, tile);

                    server.MonsterFactory.Attach(monster);

                    tile.AddContent(monster);
                }

                foreach (var xmlNpc in spawn.Npcs)
                {
                    Tile tile = GetTile(xmlNpc.Position);

                    Npc npc = server.NpcFactory.Create(xmlNpc.Name, tile);

                    server.NpcFactory.Attach(npc);

                    tile.AddContent(npc);
                }
            }

            observers = new HashSet<Creature>[ (int)Math.Ceiling( (maxY - minY + 1) / 14.0) ][];

            for (int j = 0; j < observers.Length; j++)
            {
                observers[j] = new HashSet<Creature>[ (int)Math.Ceiling( (maxX - minX + 1) / 18.0) ];

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
            int j = (position.Y - minY + 1) / 14;

            int i = (position.X - minX + 1) / 18;

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
            int j = (position.Y - minY + 1) / 14;

            int i = (position.X - minX + 1) / 18;

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
                if (a < 0)
                {
                    return -1;
                }

                return a / b;
            }

            HashSet<Creature> GetObservers(int j, int i)
            {
                if (j < 0 || j > observers.Length)
                {
                    return null;
                }

                if (i < 0 || i > observers[0].Length)
                {
                    return null;
                }

                return observers[j][i];
            }

            HashSet<Creature> creatures = new HashSet<Creature>();

            int minJ = Div(position.Y - 13 - minY + 1, 14);

            int maxJ = Div(position.Y + 14 - minY + 1, 14);

            int minI = Div(position.X - 17 - minX + 1, 18);

            int maxI = Div(position.X + 18 - minX + 1, 18);

            for (int j = minJ; j <= maxJ; j++)
            {
                for (int i = minI; i <= maxI; i++)
                {
                    HashSet<Creature> observers = GetObservers(j, i);

                    if (observers != null)
                    {
                        creatures.UnionWith(observers);
                    }
                }
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