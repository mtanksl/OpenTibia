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

            this.tiles = new Dictionary<Position, Tile>(otbmFile.Areas.Sum(area => area.Tiles.Count) );

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
                                else if (item is ReadableItem readableItem)
                                {
                                    readableItem.Text = otbmItem.Text;
                                }

                                parent.AddContent(item);
                            }
                        }

                        AddItems(tile, otbmTile.Items);
                    }
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
    }
}