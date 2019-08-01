using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Otbm;
using OpenTibia.Game;
using System.Collections.Generic;
using System.Linq;
using OtbmItem = OpenTibia.FileFormats.Otbm.Item;

namespace OpenTibia.Common.Objects
{
    public class Map : IMap
    {
        public Map(ItemFactory itemFactory, OtbmFile otbmFile)
        {
            this.tiles = new Dictionary<Position, Tile>(otbmFile.Areas.Sum(area => area.Tiles.Count) );

            foreach (var otbmArea in otbmFile.Areas)
            {
                foreach (var otbmTile in otbmArea.Tiles)
                {
                    Tile tile = new Tile(otbmArea.Position.Offset(otbmTile.OffsetX, otbmTile.OffsetY, 0) );

                    tiles.Add(tile.Position, tile);

                    if (otbmTile.OpenTibiaItemId > 0)
                    {
                        Item ground = itemFactory.Create(otbmTile.OpenTibiaItemId);
                        
                        tile.AddContent(ground);
                    }

                    if (otbmTile.Items != null)
                    {
                        void AddItems(IContainer rootContainer, List<OtbmItem> items)
                        {
                            foreach (var otbmItem in items)
                            {
                                Item item = itemFactory.Create(otbmItem.OpenTibiaId);

                                if (item is Container)
                                {
                                    Container container = (Container)item;

                                    if (otbmItem.Items != null)
                                    {
                                        AddItems(container, otbmItem.Items);
                                    }
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

                                rootContainer.AddContent(item);
                            }
                        }

                        AddItems(tile, otbmTile.Items);
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

        public IEnumerable<Tile> GetTiles()
        {
            return tiles.Values;
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