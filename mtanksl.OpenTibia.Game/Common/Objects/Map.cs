using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Otbm;
using OpenTibia.Game;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class Map : IMap
    {
        private Server server;

        public Map(Server server, OtbmFile otbmFile)
        {
            this.server = server;

            this.tiles = new Dictionary<Position, Tile>(otbmFile.Areas.Sum(area => area.Tiles.Count) );

            foreach (var otbmArea in otbmFile.Areas)
            {
                foreach (var otbmTile in otbmArea.Tiles)
                {
                    Tile tile = new Tile(otbmArea.Position.Offset(otbmTile.OffsetX, otbmTile.OffsetY, 0) );

                    tiles.Add(tile.Position, tile);

                    if (otbmTile.OpenTibiaItemId > 0)
                    {
                        Item ground = server.ItemFactory.Create(otbmTile.OpenTibiaItemId);
                        
                        tile.AddContent(ground);
                    }

                    if (otbmTile.Items != null)
                    {
                        foreach (var otbmItem in otbmTile.Items)
                        {
                            Item item = server.ItemFactory.Create(otbmItem.OpenTibiaId);

                            if (item is Container)
                            {
                                Container container = (Container)item;

                                
                            }
                            else if (item is StackableItem)
                            {
                                StackableItem stackable = (StackableItem)item;

                                stackable.Count = otbmItem.Count;
                            }
                            else if (item is TeleportItem)
                            {
                                TeleportItem teleport = (TeleportItem)item;

                                teleport.Position = otbmItem.TeleportPosition;
                            }

                            tile.AddContent(item);
                        }
                    }
                }
            }
        }

        private Dictionary<Position, Tile> tiles;

        public Tile GetTile(Position position)
        {
            Tile tile;

            tiles.TryGetValue(position, out tile);

            return tile;
        }

        public Tile GetNextTile(Tile toTile)
        {
            switch (toTile.FloorChange)
            {
                case FloorChange.Down:

                    toTile = server.Map.GetTile( toTile.Position.Offset(0, 0, 1) );

                    if (toTile != null)
                    {
                        toTile = GetNextTileDown(toTile);
                    }

                    break;

                case FloorChange.East:

                    toTile = server.Map.GetTile( toTile.Position.Offset(1, 0, -1) );

                    break;

                case FloorChange.North:

                    toTile = server.Map.GetTile( toTile.Position.Offset(0, -1, -1) );

                    break;

                case FloorChange.West:

                    toTile = server.Map.GetTile( toTile.Position.Offset(-1, 0, -1) );

                    break;

                case FloorChange.South:

                    toTile = server.Map.GetTile( toTile.Position.Offset(0, 1, -1) );

                    break;

                case FloorChange.NorthEast:

                    toTile = server.Map.GetTile( toTile.Position.Offset(1, -1, -1) );

                    break;

                case FloorChange.NorthWest:

                    toTile = server.Map.GetTile( toTile.Position.Offset(-1, -1, -1) );

                    break;

                case FloorChange.SouthEast:

                    toTile = server.Map.GetTile( toTile.Position.Offset(1, 1, -1) );

                    break;

                case FloorChange.SouthWest:

                    toTile = server.Map.GetTile( toTile.Position.Offset(-1, 1, -1) );

                    break;
            }

            return toTile;
        }

        public Tile GetNextTileDown(Tile toTile)
        {
            switch (toTile.FloorChange)
            {
                case FloorChange.East:

                    toTile = server.Map.GetTile( toTile.Position.Offset(-1, 0, 0) );

                    break;

                case FloorChange.North:

                    toTile = server.Map.GetTile( toTile.Position.Offset(0, 1, 0) );

                    break;

                case FloorChange.West:

                    toTile = server.Map.GetTile( toTile.Position.Offset(1, 0, 0) );

                    break;

                case FloorChange.South:

                    toTile = server.Map.GetTile( toTile.Position.Offset(0, -1, 0) );

                    break;

                case FloorChange.NorthEast:

                    toTile = server.Map.GetTile( toTile.Position.Offset(-1, 1, 0) );

                    break;

                case FloorChange.NorthWest:

                    toTile = server.Map.GetTile( toTile.Position.Offset(1, 1, 0) );

                    break;

                case FloorChange.SouthEast:

                    toTile = server.Map.GetTile( toTile.Position.Offset(-1, -1, 0) );

                    break;

                case FloorChange.SouthWest:

                    toTile = server.Map.GetTile( toTile.Position.Offset(1, -1, 0) );

                    break;
            }

            return toTile;
        }

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

        private Dictionary<uint, Creature> creatures = new Dictionary<uint, Creature>();

        public void AddCreature(Creature creature)
        {
            if (creature.Id == 0)
            {
                creature.Id = GenerateId();
            }

            creatures.Add(creature.Id, creature);
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