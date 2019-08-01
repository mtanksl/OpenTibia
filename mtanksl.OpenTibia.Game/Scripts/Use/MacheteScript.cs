using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class MacheteScript : IItemUseWithItemScript
    {
        private HashSet<ushort> machetes = new HashSet<ushort>() { 2420 };

        private Dictionary<ushort, ushort> jungleGrass = new Dictionary<ushort, ushort>()
        {
            { 2780, 2781 }
        };

        public void Register(Server server)
        {
            foreach (var openTibiaId in machetes)
            {
                server.Scripts.ItemUseWithItemScripts.Add(openTibiaId, this);
            }
        }

        public bool NextTo
        {
            get
            {
                return true;
            }
        }

        public bool OnItemUseWithItem(Player player, Item fromItem, Item toItem, Server server, CommandContext context)
        {
            ushort toOpenTibiaId;

            if (jungleGrass.TryGetValue(toItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                new ItemTransformCommand(toItem, toOpenTibiaId).Execute(server, context);

                return true;
            }
            
            return false;
        }
    }
}