using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class ShallowWaterScript : IItemMoveScript
    {
        private HashSet<ushort> shallowWaters = new HashSet<ushort>() { 4608, 4609, 4610, 4611, 4612, 4613, 4614, 4615, 4616, 4617, 4618, 4619, 4620, 4621, 4622, 4623, 4624, 4625 };

        public void Start(Server server)
        {
            server.Scripts.ItemMoveScripts.Add(this);
        }

        public void Stop(Server server)
        {

        }

        public bool OnItemMove(Player player, Item fromItem, IContainer toContainer, byte toIndex, byte count, Context context)
        {
            if (toContainer is Tile toTile && toTile.Ground != null && shallowWaters.Contains(toTile.Ground.Metadata.OpenTibiaId) )
            {
                new ItemDestroyCommand(fromItem).Execute(context);

                new MagicEffectCommand(toTile.Position, MagicEffectType.BlueRings).Execute(context);

                return true;
            }

            return false;
        }        
    }
}