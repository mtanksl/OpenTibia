using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class ShovelScript : ItemUseWithItemScript
    {
        private static HashSet<ushort> shovels = new HashSet<ushort>() { 2554 };

        private static Dictionary<ushort, ushort> stonePiles = new Dictionary<ushort, ushort>()
        {
            { 468, 469 },
            { 481, 482 },
            { 483, 484 }
        };

        public override void Register(Server server)
        {
            foreach (var openTibiaId in shovels)
            {
                server.ItemUseWithItemScripts.Add(openTibiaId, this);
            }
        }

        public override bool Execute(Player player, Item fromItem, Item toItem, Server server, CommandContext context)
        {
            Position fromPosition = player.Tile.Position;

            Position toPosition = null;

            switch ( toItem.GetRootContainer() )
            {
                case Tile tile:

                    toPosition = tile.Position;

                    break;

                case Inventory inventory:

                    toPosition = inventory.Player.Tile.Position;

                    break;
            }

            if ( toPosition != null && fromPosition.IsNextTo(toPosition) )
            {
                ushort toOpenTibiaId;

                if (stonePiles.TryGetValue(toItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    ItemTransformCommand command = new ItemTransformCommand(toItem, toOpenTibiaId);

                    command.Execute(server, context);

                    return true;
                }
            }            
            
            return false;
        }
    }
}