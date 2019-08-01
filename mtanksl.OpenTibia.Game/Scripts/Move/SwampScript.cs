using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class SwampScript : IItemMoveScript
    {
        private HashSet<ushort> swamps = new HashSet<ushort>() { 4691, 4692, 4693, 4694, 4695, 4696, 4697, 4698, 4699, 4700, 4701, 4702, 4703, 4704, 4705, 4706, 4707, 4708, 4709, 4710, 4711, 4712 };

        public void Register(Server server)
        {
            server.Scripts.ItemMoveScripts.Add(this);
        }

        public bool OnItemMove(Player player, Item fromItem, IContainer toContainer, byte toIndex, byte count, Server server, CommandContext context)
        {
            if (toContainer is Tile toTile && toTile.Ground != null && swamps.Contains(toTile.Ground.Metadata.OpenTibiaId) )
            {
                new ItemDestroyCommand(fromItem).Execute(server, context);

                new MagicEffectCommand(toTile.Position, MagicEffectType.GreenRings).Execute(server, context);

                return true;
            }

            return false;
        }        
    }
}