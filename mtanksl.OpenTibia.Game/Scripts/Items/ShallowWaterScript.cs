using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class ShallowWaterScript : IItemMoveScript
    {
        private HashSet<ushort> shallowWaters = new HashSet<ushort>() { 4608, 4609, 4610, 4611, 4612, 4613, 4614, 4615, 4616, 4617, 4618, 4619, 4620, 4621, 4622, 4623, 4624, 4625 };

        public void Register(Server server)
        {
            foreach (var tile in server.Map.GetTiles() )
            {
                foreach (var item in tile.GetItems() )
                {
                    if ( shallowWaters.Contains(item.Metadata.OpenTibiaId) )
                    {
                        positions.Add(tile.Position);

                        break;
                    }
                }
            }

            server.ItemMoveScripts.Add(this);
        }

        private HashSet<Position> positions = new HashSet<Position>();

        public bool Execute(Player player, Item fromItem, IContainer toContainer, byte toIndex, byte count, Server server, CommandContext context)
        {
            if (toContainer is Tile toTile)
            {
                if ( positions.Contains(toTile.Position) )
                {
                    new ItemDestroyCommand(fromItem).Execute(server, context);

                    new MagicEffectCommand(toTile.Position, MagicEffectType.BlueRings).Execute(server, context);

                    return true;
                }
            }

            return false;
        }        
    }
}