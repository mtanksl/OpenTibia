using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class DefaultItemRotateScript : ItemRotateScript
    {
        private static Dictionary<ushort, ushort> items = new Dictionary<ushort, ushort>()
        {
            //Oven

            { 6356, 6358 },
            { 6358, 6360 },
            { 6360, 6362 },
            { 6362, 6356 },

            //Oven

            { 6357, 6359 },
            { 6359, 6361 },
            { 6361, 6363 },
            { 6363, 6357 }
        };

        public override void Register(Server server)
        {
            foreach (var item in items)
            {
                server.ItemRotateScripts.Add(item.Key, this);
            }
        }

        public override bool Execute(Player player, Item fromItem, Server server, CommandContext context)
        {
            ushort toOpenTibiaId;

            if ( items.TryGetValue(fromItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                ItemTransformCommand command = new ItemTransformCommand(fromItem, toOpenTibiaId);

                command.Execute(server, context);

                return true;
            }

            return false;
        }
    }
}