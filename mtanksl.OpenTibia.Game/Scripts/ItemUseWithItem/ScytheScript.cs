using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class ScytheScript : IItemUseWithItemScript
    {
        private HashSet<ushort> scythes = new HashSet<ushort>() { 2550 };

        private Dictionary<ushort, ushort> wheats = new Dictionary<ushort, ushort>()
        {
            { 2739, 2737 }
        };

        private ushort wheat = 2694;

        public void Start(Server server)
        {
            foreach (var openTibiaId in scythes)
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

        public bool OnItemUseWithItem(Player player, Item item, Item toItem, Server server, Context context)
        {
            ushort toOpenTibiaId;

            if (wheats.TryGetValue(toItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                Tile tile = (Tile)toItem.Container;

                new ItemTransformCommand(toItem, toOpenTibiaId).Execute(server, context);

                new ItemCreateCommand(wheat, tile.Position).Execute(server, context);

                return true;
            }
            
            return false;
        }
    }
}