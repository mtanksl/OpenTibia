using System;
using System.Linq;
using System.Collections.Generic;

using OpenTibia.Otbm;
using OpenTibia.IO;

namespace OpenTibia
{
    public class Map
    {
        public Map(OtbmFile otbmFile)
        {
            tiles = new Dictionary<Position, Tile>(otbmFile.Areas.Sum(area => area.Tiles.Count) );
            
            foreach (var otbmArea in otbmFile.Areas)
            {
                foreach (var otbmTile in otbmArea.Tiles)
                {
                    Tile tile = new Tile(otbmArea.Position.Offset(otbmTile.OffsetX, otbmTile.OffsetY, 0) );

                    tiles.Add(tile.Position, tile);

                    if (otbmTile.ItemId > 0)
                    {
                        Item ground = Game.Current.ItemFactory.Create(otbmTile.ItemId);
                        
                        tile.AddContent(ground);
                    }

                    if (otbmTile.Items != null)
                    {
                        foreach (var otbmItem in otbmTile.Items)
                        {
                            Item item = Game.Current.ItemFactory.Create(otbmItem.Id);

                            item.ActionId = otbmItem.ActionId;

                            item.UniqueId = otbmItem.UniqueId;

                            if (item is Container)
                            {
                                Container container = (Container)item;

                                //TODO: Load container items
                            }
                            else if (item is Stackable)
                            {
                                Stackable stackable = (Stackable)item;

                                stackable.Count = otbmItem.Count;
                            }
                            else if (item is Teleport)
                            {
                                Teleport teleport = (Teleport)item;

                                teleport.Position = otbmItem.TeleportPosition;
                            }

                            tile.AddContent(item);
                        }
                    }
                }
            }

            towns = new List<Town>(otbmFile.Towns.Count);

            foreach (var otbmTown in otbmFile.Towns)
            {
                Town town = new Town()
                {
                    Id = otbmTown.Id,

                    Name = otbmTown.Name,

                    Position = otbmTown.Position
                };

                towns.Add(town);
            }
        }

        public Map(string path)
        {
            using (ByteArrayFileTreeStream stream = new ByteArrayFileTreeStream(path) )
            {
                ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);
            
                LoadOtbmInfo(stream, reader);

                if ( stream.Child() )
                {
                    LoadMapInfo(stream, reader);

                    if ( stream.Child() )
                    {
                        tiles = new Dictionary<Position, Tile>();

                        while(true)
                        {
                            switch ( (OtbmType)reader.ReadByte() )
                            {
                                case OtbmType.Area:

                                    LoadArea(stream, reader);

                                    break;

                                case OtbmType.Towns:

                                    if ( stream.Child() )
                                    {
                                        towns = new List<Town>();

                                        while (true)
                                        {
                                            towns.Add( LoadTown(stream, reader) );

                                            if ( !stream.Next() )
                                            {
                                                break;
                                            }
                                        }
                                    }

                                    break;

                                case OtbmType.Waypoints:

                                    if ( stream.Child() )
                                    {
                                        //TODO: Load waypoints

                                        while (true)
                                        {
                                            LoadWaypoint(stream, reader);

                                            if ( !stream.Next() )
                                            {
                                                break;
                                            }
                                        }                                
                                    }

                                    break;
                            }

                            if ( !stream.Next() )
                            {
                                break;
                            }
                        } 
                    }               
                }
            }
        }

        private void LoadOtbmInfo(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            stream.Seek(Origin.Current, 6);

            reader.ReadUInt();

            reader.ReadUShort();

            reader.ReadUShort();

            reader.ReadUInt();

            reader.ReadUInt();
        }

        private void LoadMapInfo(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            stream.Seek(Origin.Current, 1);

            while (true)
            {
                switch ( (OtbmAttribute)reader.ReadByte() )
                {
                    case OtbmAttribute.Description:

                        reader.ReadString();

                        break;

                    case OtbmAttribute.SpawnFile:

                        reader.ReadString();

                        break;

                    case OtbmAttribute.HouseFile:

                        reader.ReadString();

                        break;

                    default:

                        stream.Seek(Origin.Current, -1);

                        return;
                }
            }
        }

        private void LoadArea(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            Position position = new Position(reader.ReadUShort(), reader.ReadUShort(), reader.ReadByte() );

            if ( stream.Child() )
            {
                while (true)
                {
                    Tile tile = null;

                    switch ( (OtbmType)reader.ReadByte() )
                    {
                        case OtbmType.Tile:

                            tile = LoadTile(position, stream, reader);

                            break;

                        case OtbmType.HouseTile:

                            tile = LoadHouseTile(position, stream, reader);

                            break;
                    }
                    tiles.Add(tile.Position, tile);
                    
                    if ( !stream.Next() )
                    {
                        break; 
                    }
                }
            }
        }

        private Tile LoadTile(Position position, ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            Tile tile = new Tile(position.Offset(reader.ReadByte(), reader.ReadByte(), 0) );

            while (true)
            {
                switch ( (OtbmAttribute)reader.ReadByte() )
                {
                    case OtbmAttribute.Flags:

                        reader.ReadUInt();

                        break;

                    case OtbmAttribute.ItemId:
                               
                        Item ground = Game.Current.ItemFactory.Create( reader.ReadUShort() );

                        tile.AddContent(ground);

                        break;
                        
                    default:

                        stream.Seek(Origin.Current, -1);
                        
                        if ( stream.Child() )
                        {
                            while (true)
                            {
                                tile.AddContent( LoadItem(stream, reader) );

                                if ( !stream.Next() )
                                {
                                    break;
                                }
                            }
                        }
                        return tile;
                }
            }
        }

        private Tile LoadHouseTile(Position position, ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            Tile tile = new Tile(position.Offset(reader.ReadByte(), reader.ReadByte(), 0) );

            reader.ReadUInt();

            while (true)
            {
                switch ( (OtbmAttribute)reader.ReadByte() )
                {
                    case OtbmAttribute.Flags:

                        reader.ReadUInt();

                        break;

                    case OtbmAttribute.ItemId:
                               
                        Item ground = Game.Current.ItemFactory.Create( reader.ReadUShort() );

                        tile.AddContent(ground);

                        break;
                        
                    default:

                        stream.Seek(Origin.Current, -1);
                        
                        if ( stream.Child() )
                        {
                            while (true)
                            {
                                tile.AddContent( LoadItem(stream, reader) );

                                if ( !stream.Next() )
                                {
                                    break;
                                }
                            }
                        }
                        return tile;
                }
            }
        }

        private Item LoadItem(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            stream.Seek(Origin.Current, 1);

            Item item = Game.Current.ItemFactory.Create( reader.ReadUShort() );

            while (true)
            {
                switch ( (OtbmAttribute)reader.ReadByte() )
                {
                    case OtbmAttribute.Count:

                        byte count = reader.ReadByte();

                        if (item is Stackable)
                        {
                            Stackable stackable = (Stackable)item;

                            stackable.Count = count;
                        }
                        break;

                    case OtbmAttribute.ActionId:

                        item.ActionId = reader.ReadUShort();

                        break;

                    case OtbmAttribute.UniqueId:

                        item.UniqueId = reader.ReadUShort();

                        break;

                    case OtbmAttribute.Text:

                        reader.ReadString();

                        break;

                    case OtbmAttribute.WrittenDate:

                        reader.ReadUInt();

                        break;

                    case OtbmAttribute.WrittenBy:

                        reader.ReadString();

                        break;

                    case OtbmAttribute.SpecialDescription:

                        reader.ReadString();

                        break;

                    case OtbmAttribute.RuneCharges:

                        reader.ReadByte();

                        break;

                    case OtbmAttribute.Charges:

                        reader.ReadUShort();

                        break;

                    case OtbmAttribute.Duration:

                        reader.ReadUInt();

                        break;

                    case OtbmAttribute.Decaying:

                        reader.ReadByte();

                        break;

                    case OtbmAttribute.DepotId:

                        reader.ReadUShort();

                        break;

                    case OtbmAttribute.HouseDoorId:

                        reader.ReadByte();

                        break;

                    case OtbmAttribute.SleeperId:

                        reader.ReadUInt();

                        break;

                    case OtbmAttribute.SleepStart:

                        reader.ReadUInt();

                        break;

                    case OtbmAttribute.TeleportDestination:

                        Position position = new Position(reader.ReadUShort(), reader.ReadUShort(), reader.ReadByte() );

                        if (item is Teleport)
                        {
                            Teleport teleport = (Teleport)item;

                            teleport.Position = position;
                        }
                        break;

                    case OtbmAttribute.ContainerItems:

                        reader.ReadUInt();

                        break;

                    default:

                        stream.Seek(Origin.Current, -1);

                        if ( stream.Child() )
                        {
                            while (true)
                            {
                                Item sub = LoadItem(stream, reader);

                                if (item is Container)
                                {
                                    Container container = (Container)item;

                                    container.AddContent(sub);
                                }

                                if ( !stream.Next() )
                                {
                                    break;
                                }
                            }
                        }
                        return item;
                }
            }
        }

        private Town LoadTown(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            Town town = new Town();

            stream.Seek(Origin.Current, 1);

            town.Id = reader.ReadUInt();

            town.Name = reader.ReadString();

            town.Position = new Position(reader.ReadUShort(), reader.ReadUShort(), reader.ReadByte() );

            return town;
        }

        private void LoadWaypoint(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            stream.Seek(Origin.Current, 1);

            reader.ReadString();

            reader.ReadUShort();

            reader.ReadUShort();

            reader.ReadByte();
        }

        private Dictionary<Position, Tile> tiles;

        public Tile GetTile(Position position)
        {
            Tile tile;

            tiles.TryGetValue(position, out tile);

            return tile;
        }

        private List<Town> towns;

        public Town GetTown(uint townId)
        {
            return towns.Where(t => t.Id == townId).FirstOrDefault();
        }

        private Dictionary<uint, Creature> creatures = new Dictionary<uint, Creature>();

        private uint uniqueId = 0;

        private uint GenerateId()
        {
            uniqueId++;

            if (uniqueId == 0)
            {
                uniqueId++;
            }

            return uniqueId;
        }

        public T AddCreature<T>(T creature) where T : Creature
        {
            creature.Id = GenerateId();

            creatures.Add(creature.Id, creature);

            return creature;
        }

        public void RemoveCreature(Creature creature)
        {
            creatures.Remove(creature.Id);
        }
        
        public Creature GetCreature(uint creatureId)
        {
            Creature creature;

            creatures.TryGetValue(creatureId, out creature);

            return creature;
        }

        public Player GetPlayer(string name)
        {
            return GetPlayers().Where(player => player.Name == name).FirstOrDefault();
        }

        public IEnumerable<Creature> GetCreatures()
        {
            return creatures.Values;
        }

        public IEnumerable<Monster> GetMonsters()
        {
            return creatures.Values.OfType<Monster>();
        }

        public IEnumerable<Npc> GetNpcs()
        {
            return creatures.Values.OfType<Npc>();
        }

        public IEnumerable<Player> GetPlayers()
        {
            return creatures.Values.OfType<Player>();
        }
    }
}