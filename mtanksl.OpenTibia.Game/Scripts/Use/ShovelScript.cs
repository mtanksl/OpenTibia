using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class ShovelScript : IItemUseWithItemScript
    {
        private HashSet<ushort> shovels = new HashSet<ushort>() { 2554, 5710 };

        private Dictionary<ushort, ushort> stonePiles = new Dictionary<ushort, ushort>()
        {
            { 468, 469 },
            { 481, 482 },
            { 483, 484 }
        };

        public void Register(Server server)
        {
            foreach (var openTibiaId in shovels)
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

            if (stonePiles.TryGetValue(toItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                new ItemTransformCommand(toItem, toOpenTibiaId).Execute(server, context);

                return true;
            }
            
            return false;
        }
    }
}