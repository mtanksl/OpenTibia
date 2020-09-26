using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class DustbinScript : IItemMoveScript
    {
        private HashSet<ushort> dustbins = new HashSet<ushort>() { 1777 };

        private HashSet<Position> positions = new HashSet<Position>();

        public void Start(Server server)
        {
            foreach (var tile in server.Map.GetTiles() )
            {
                foreach (var item in tile.GetItems() )
                {
                    if ( dustbins.Contains(item.Metadata.OpenTibiaId) )
                    {
                        positions.Add(tile.Position);

                        break;
                    }
                }
            }

            server.Scripts.ItemMoveScripts.Add(this);
        }

        public void Stop(Server server)
        {

        }

        public bool OnItemMove(Player player, Item fromItem, IContainer toContainer, byte toIndex, byte count, Context context)
        {
            if (toContainer is Tile toTile && positions.Contains(toTile.Position) )
            {
                new ItemDestroyCommand(fromItem).Execute(context);

                return true;
            }

            return false;
        }        
    }
}