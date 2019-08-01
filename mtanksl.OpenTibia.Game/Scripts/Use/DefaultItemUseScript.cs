using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class DefaultItemUseScript : IItemUseScript
    {
        private Dictionary<ushort, ushort> items = new Dictionary<ushort, ushort>()
        {
            { 6356, 6357 },
            { 6357, 6356 },
            { 6358, 6359 },
            { 6359, 6358 },
            { 6360, 6361 },
            { 6361, 6360 },
            { 6362, 6363 },
            { 6363, 6362 }
        };

        public void Register(Server server)
        {
            foreach (var item in items)
            {
                server.Scripts.ItemUseScripts.Add(item.Key, this);
            }
        }

        public bool OnItemUse(Player player, Item fromItem, Server server, CommandContext context)
        {
            ushort toOpenTibiaId;

            if ( items.TryGetValue(fromItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                new ItemTransformCommand(fromItem, toOpenTibiaId).Execute(server, context);

                return true;
            }

            return false;
        }
    }
}