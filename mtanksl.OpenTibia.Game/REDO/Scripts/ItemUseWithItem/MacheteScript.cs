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
            { 2782, 2781 }
        };

        public void Start(Server server)
        {
            foreach (var openTibiaId in machetes)
            {
                server.Scripts.ItemUseWithItemScripts.Add(openTibiaId, this);
            }
        }

        public void Stop(Server server)
        {

        }

        public bool NextTo
        {
            get
            {
                return true;
            }
        }

        public bool OnItemUseWithItem(Player player, Item item, Item toItem, Context context)
        {
            ushort toOpenTibiaId;

            if (jungleGrass.TryGetValue(toItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                new ItemTransformCommand(toItem, toOpenTibiaId, 1).Execute(context);

                return true;
            }
            
            return false;
        }
    }
}