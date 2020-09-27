using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class LavaScript : IItemMoveScript
    {
        private HashSet<ushort> lavas = new HashSet<ushort>() { 598, 599, 600, 601 };

        public void Start(Server server)
        {
            server.Scripts.ItemMoveScripts.Add(this);
        }

        public void Stop(Server server)
        {

        }

        public bool OnItemMove(Player player, Item fromItem, IContainer toContainer, byte toIndex, byte count, Context context)
        {
            if (toContainer is Tile toTile && toTile.Ground != null && lavas.Contains(toTile.Ground.Metadata.OpenTibiaId) )
            {
                new ItemDestroyCommand(fromItem).Execute(context);

                new MagicEffectCommand(toTile.Position, MagicEffectType.FirePlume).Execute(context);

                return true;
            }

            return false;
        }        
    }
}