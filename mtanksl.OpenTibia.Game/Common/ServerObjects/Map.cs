using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Otbm;
using OpenTibia.FileFormats.Xml.Houses;
using OpenTibia.FileFormats.Xml.Spawns;
using OpenTibia.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using OtbmHouseTile = OpenTibia.FileFormats.Otbm.HouseTile;
using OtbmItem = OpenTibia.FileFormats.Otbm.Item;

namespace OpenTibia.Common.Objects
{
    public class Map : IMap
    {
        private class PositionEqualityComparer : IEqualityComparer<Position>
        {
            public bool Equals(Position objA, Position objB)
            {
                return objA.Equals(objB);
            }

            public int GetHashCode(Position obj)
            {
                return obj.X << 16 | obj.Y;
            }
        }

        private class TileCollection
        {
            public Dictionary<Position, Tile>[] floors;

            public TileCollection()
            {
                floors = new Dictionary<Position, Tile>[16];

                for (int i = 0; i < floors.Length; i++)
                {
                    floors[i] = new Dictionary<Position, Tile>(new PositionEqualityComparer() );
                }
            }

            public void AddTile(Position position, Tile tile)
            {
                floors[position.Z].Add(position, tile);
            }

            public Tile GetTile(Position position)
            {
                Tile tile;

                floors[position.Z].TryGetValue(position, out tile);

                return tile;
            }

            public IEnumerable<Tile> GetTiles()
            {
                foreach (var floor in floors)
                {
                    foreach (var tile in floor.Values)
                    {
                        yield return tile;
                    }
                }
            }
        }

        private IServer server;

        private int minX = int.MaxValue;

        private int minY = int.MaxValue;

        private int maxX = 0;

        private int maxY = 0;

        public Map(IServer server)
        {
            this.server = server;
        }

        public void Start(OtbmFile otbmFile, SpawnFile spawnFile, HouseFile houseFile)
        {
            if (otbmFile.Towns != null)
            {
                this.townsByName = new Dictionary<string, Town>(otbmFile.Towns.Count);

                this.townsById = new Dictionary<ushort, Town>(otbmFile.Towns.Count);

                foreach (var otbmTown in otbmFile.Towns)
                {
                    Town town = new Town()
                    {
                        Id = (ushort)otbmTown.Id,

                        Name = otbmTown.Name,

                        Position = otbmTown.Position
                    };

                    townsByName.Add(town.Name, town);

                    townsById.Add(town.Id, town);
                }
            }
            else
            {
                this.townsByName = new Dictionary<string, Town>();

                this.townsById = new Dictionary<ushort, Town>();
            }

            if (otbmFile.Waypoints != null)
            {
                this.waypointsByName = new Dictionary<string, Waypoint>(otbmFile.Waypoints.Count);

                foreach (var otbmWaypoint in otbmFile.Waypoints)
                {
                    Waypoint waypoint = new Waypoint()
                    {
                        Name = otbmWaypoint.Name,

                        Position = otbmWaypoint.Position
                    };

                    waypointsByName.Add(waypoint.Name, waypoint);
                }
            }
            else
            {
                this.waypointsByName = new Dictionary<string, Waypoint>();
            }

            if (houseFile.Houses != null)
            {
                this.housesByName = new Dictionary<string, House>(houseFile.Houses.Count);

                this.housesById = new Dictionary<ushort, House>(houseFile.Houses.Count);

                foreach (var xmlHouse in houseFile.Houses)
                {
                    Town town = GetTown( (ushort)xmlHouse.TownId);

                    if (town != null)
                    {
                        House house = new House()
                        {
                            Id = (ushort)xmlHouse.Id,

                            Name = xmlHouse.Name,

                            Entry = xmlHouse.Entry,

                            Town = town,

                            Rent = xmlHouse.Rent,

                            Size = (ushort)xmlHouse.Size,

                            Guildhall = xmlHouse.Guildhall
                        };



                        housesByName.Add(house.Name, house);

                        housesById.Add(house.Id, house);
                    }
                }
            }
            else
            {
                this.housesByName = new Dictionary<string, House>();

                this.housesById = new Dictionary<ushort, House>();
            }          

            foreach (var otbmArea in otbmFile.Areas)
            {
                foreach (var otbmTile in otbmArea.Tiles)
                {
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

            this.tiles = new TileCollection();

            foreach (var otbmArea in otbmFile.Areas)
            {
                foreach (var otbmTile in otbmArea.Tiles)
                {
                    Position position = otbmArea.Position.Offset(otbmTile.OffsetX, otbmTile.OffsetY, 0);

                    Tile tile;

                    if (otbmTile is OtbmHouseTile otbmHouseTile)
                    {
                        House house = GetHouse( (ushort)otbmHouseTile.HouseId);

                        if (house != null)
                        {
                            HouseTile houseTile = new HouseTile(position)
                            {
                                House = house
                            };

                            house.AddTile(position, houseTile);

                            tile = houseTile;
                        }
                        else
                        {
                            tile = new Tile(position);
                        }
                    }
                    else
                    {
                        tile = new Tile(position);
                    }

                    tile.ProtectionZone = (otbmTile.Flags & TileFlags.ProtectionZone) == TileFlags.ProtectionZone;

                    tile.NoLogoutZone = (otbmTile.Flags & TileFlags.NoLogoutZone) == TileFlags.NoLogoutZone;

                    tiles.AddTile(position, tile);

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

                                        locker.Town = GetTown(otbmItem.DepotId);

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

                                    case SignItem signItem:

                                        signItem.Text = otbmItem.Text;

                                        break;
                                }

                                parent.AddContent(item);
                            }
                        }

                        AddItems(tile, otbmTile.Items);
                    }
                }
            }

            foreach (var xmlSpawn in spawnFile.Spawns)
            {
                foreach (var xmlMonster in xmlSpawn.Monsters)
                {
                    Tile tile = GetTile(xmlMonster.Position);

                    Monster monster = server.MonsterFactory.Create(xmlMonster.Name, tile);

                    if (monster != null)
                    {
                        server.MonsterFactory.Attach(monster);

                        tile.AddContent(monster);
                    }
                    else
                    {
                        unknownMonsters.Add(xmlMonster.Name);
                    }
                }

                foreach (var xmlNpc in xmlSpawn.Npcs)
                {
                    Tile tile = GetTile(xmlNpc.Position);

                    Npc npc = server.NpcFactory.Create(xmlNpc.Name, tile);

                    if (npc != null)
                    {
                        server.NpcFactory.Attach(npc);

                        tile.AddContent(npc);
                    }
                    else
                    {
                        unknownNpcs.Add(xmlNpc.Name);
                    }
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

        private HashSet<string> unknownMonsters = new HashSet<string>();

        public HashSet<string> UnknownMonsters
        {
            get
            {
                return unknownMonsters;
            }
        }

        private HashSet<string> unknownNpcs = new HashSet<string>();

        public HashSet<string> UnknownNpcs
        {
            get
            {
                return unknownNpcs;
            }
        }

        private Dictionary<string, Town> townsByName;

        private Dictionary<ushort, Town> townsById;

        public Town GetTown(string name)
        {
            Town town;

            townsByName.TryGetValue(name, out town);

            return town;
        }

        public Town GetTown(ushort townId)
        {
            Town town;

            townsById.TryGetValue(townId, out town);

            return town;
        }

        public IEnumerable<Town> GetTowns()
        {
            return townsByName.Values;
        }

        private Dictionary<string, Waypoint> waypointsByName;

        public Waypoint GetWaypoint(string name)
        {
            Waypoint waypoint;

            waypointsByName.TryGetValue(name, out waypoint);

            return waypoint;
        }

        public IEnumerable<Waypoint> GetWaypoints()
        {
            return waypointsByName.Values;
        }

        private Dictionary<string, House> housesByName;

        private Dictionary<ushort, House> housesById;

        public House GetHouse(string name)
        {
            House house;

            housesByName.TryGetValue(name, out house);

            return house;
        }

        public House GetHouse(ushort houseId)
        {
            House house;

            housesById.TryGetValue(houseId, out house);

            return house;
        }

        public IEnumerable<House> GetHouses()
        {
            return housesByName.Values;
        }

        private TileCollection tiles;

        public Tile GetTile(Position position)
        {
            return tiles.GetTile(position);
        }

        public IEnumerable<Tile> GetTiles()
        {
            return tiles.GetTiles();
        }

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
                if (j < 0 || j > observers.Length - 1)
                {
                    return null;
                }

                if (i < 0 || i > observers[0].Length - 1)
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