using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class DefaultItemRotateScript : IItemRotateScript
    {
        private Dictionary<ushort, ushort> items = new Dictionary<ushort, ushort>()
        {
            { 6356, 6358 },
            { 6358, 6360 },
            { 6360, 6362 },
            { 6362, 6356 },

            { 6357, 6359 },
            { 6359, 6361 },
            { 6361, 6363 },
            { 6363, 6357 }
        };

        public void Start(Server server)
        {
            foreach (var item in items)
            {
                server.Scripts.ItemRotateScripts.Add(item.Key, this);
            }
        }

        public void Stop(Server server)
        {

        }

        public bool OnItemRotate(Player player, Item fromItem, Server server, Context context)
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