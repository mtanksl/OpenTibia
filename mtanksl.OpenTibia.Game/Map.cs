using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game
{
    public class Map : IMap
    {
        public Map(ItemFactory itemFactory, FileFormats.Otbm.OtbmFile otbmFile)
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
                        Item ground = itemFactory.Create(otbmTile.ItemId);
                        
                        tile.AddContent(ground);
                    }

                    if (otbmTile.Items != null)
                    {
                        foreach (var otbmItem in otbmTile.Items)
                        {
                            Item item = itemFactory.Create(otbmItem.Id);

                            if (item is Container)
                            {
                                Container container = (Container)item;

                                // TODO: Load container items
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
    }
}