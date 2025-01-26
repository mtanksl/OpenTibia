using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Otbm;
using OpenTibia.FileFormats.Xml.Houses;
using System;
using System.Collections.Generic;
using System.Linq;
using House = OpenTibia.Common.Objects.House;
using HouseTile = OpenTibia.Common.Objects.HouseTile;
using Item = OpenTibia.Common.Objects.Item;
using OtbmHouseTile = OpenTibia.FileFormats.Otbm.HouseTile;
using OtbmItem = OpenTibia.FileFormats.Otbm.Item;
using Tile = OpenTibia.Common.Objects.Tile;
using Town = OpenTibia.Common.Objects.Town;
using Waypoint = OpenTibia.Common.Objects.Waypoint;

namespace OpenTibia.Game.Common.ServerObjects
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

        private ushort width;

        public ushort Width
        {
            get
            {
                return width;
            }
        }

        private ushort height;

        public ushort Height
        {
            get
            {
                return height;
            }
        }

        private List<string> warnings = new List<string>();

        public List<string> Warnings
        {
            get
            {
                return warnings;
            }
        }

        public void Start(OtbmFile otbmFile, HouseFile houseFile)
        {
            width = otbmFile.OtbmInfo.Width;

            height = otbmFile.OtbmInfo.Height;

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
                            HouseTile houseTile = new HouseTile(position, (otbmTile.OpenTibiaItemId > 0 ? 1 : 0) + (otbmTile.Items != null ? otbmTile.Items.Count : 0) )
                            {
                                House = house
                            };

                            house.AddTile(position, houseTile);

                            tile = houseTile;
                        }
                        else
                        {
                            tile = new Tile(position, (otbmTile.OpenTibiaItemId > 0 ? 1 : 0) + (otbmTile.Items != null ? otbmTile.Items.Count : 0) );
                        }
                    }
                    else
                    {
                        tile = new Tile(position, (otbmTile.OpenTibiaItemId > 0 ? 1 : 0) + (otbmTile.Items != null ? otbmTile.Items.Count : 0) );
                    }

                    tile.ProtectionZone = (otbmTile.Flags & TileFlags.ProtectionZone) == TileFlags.ProtectionZone;

                    tile.NoLogoutZone = (otbmTile.Flags & TileFlags.NoLogoutZone) == TileFlags.NoLogoutZone;

                    tiles.AddTile(position, tile);

                    if (otbmTile.OpenTibiaItemId > 0)
                    {
                        Item ground = server.ItemFactory.Create(otbmTile.OpenTibiaItemId, 1);

                        if (ground != null)
                        {
                            server.ItemFactory.Attach(ground);

                            ground.LoadedFromMap = true;

                            tile.AddContent(ground);
                        }
                    }

                    if (otbmTile.Items != null)
                    {
                        void AddItems(IContainer parent, List<OtbmItem> items)
                        {
                            foreach (var otbmItem in items)
                            {
                                if (parent is HouseTile houseTile)
                                {
                                    ItemMetadata itemMetadata = server.ItemFactory.GetItemMetadataByOpenTibiaId(otbmItem.OpenTibiaId);

                                    if ( !itemMetadata.Flags.Is(ItemMetadataFlags.NotMoveable) )
                                    {
                                        warnings.Add("Moveable item found inside house at " + houseTile.Position);

                                        continue;
                                    }
                                }

                                Item item = server.ItemFactory.Create(otbmItem.OpenTibiaId, otbmItem.Count);

                                if (item != null)
                                {
                                    server.ItemFactory.Attach(item);

                                    item.LoadedFromMap = true;

                                    item.ActionId = otbmItem.ActionId;

                                    item.UniqueId = otbmItem.UniqueId;

                                    switch (item)
                                    {
                                        case TeleportItem teleport:

                                            teleport.Position = otbmItem.TeleportPosition;

                                            break;

                                        case Locker locker:

                                            locker.TownId = otbmItem.TownId;

                                            break;

                                        case Container container:

                                            if (otbmItem.Items != null)
                                            {
                                                AddItems(container, otbmItem.Items);
                                            }

                                            break;

                                        case DoorItem doorItem:

                                            doorItem.DoorId = otbmItem.DoorId;

                                            break;

                                        case ReadableItem readableItem:

                                            readableItem.Text = otbmItem.Text;

                                            readableItem.Author = otbmItem.WrittenBy;

                                            break;                                  
                                    }

                                    parent.AddContent(item);
                                }                              
                            }
                        }

                        AddItems(tile, otbmTile.Items);
                    }
                }
            }

            //TODO: Use quadtree

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