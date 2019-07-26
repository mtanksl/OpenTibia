using OpenTibia.Common.Structures;
using OpenTibia.Game;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class Map : IMap
    {
        public Map(Server server, FileFormats.Otbm.OtbmFile otbmFile)
        {
            tiles = new Dictionary<Position, Tile>(otbmFile.Areas.Sum(area => area.Tiles.Count) );

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

                                //TODO: Load container items
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