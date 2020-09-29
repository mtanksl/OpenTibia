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

                                if (item is TeleportItem teleport)
                                {
                                    teleport.Position = otbmItem.TeleportPosition;
                                }
                                else if (item is Container container)
                                {
                                    if (otbmItem.Items != null)
                                    {
                                        AddItems(container, otbmItem.Items);
                                    }
                                }
                                else if (item is StackableItem stackable)
                                {
                                    stackable.Count = otbmItem.Count;
                                }
                                else if (item is FluidItem fluidItem)
                                {
                                    fluidItem.FluidType = (FluidType)otbmItem.Count;
                                }
                                else if (item is ReadableItem readableItem)
                                {
                                    readableItem.Text = otbmItem.Text;
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
    }
}