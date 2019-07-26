using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class RopeScript : ItemUseWithItemScript
    {
        private static HashSet<ushort> ropes = new HashSet<ushort>() { 2120 };

        private static HashSet<ushort> ropeSpots = new HashSet<ushort> { 384 };

        public override void Register(Server server)
        {
            foreach (var openTibiaId in ropes)
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
                if (ropeSpots.Contains(toItem.Metadata.OpenTibiaId) )
                {
                    TeleportCommand command = new TeleportCommand(player, ( (Tile)toItem.Container ).Position.Offset(0, 1, -1) );

                    command.Execute(server, context);

                    return true;
                }   
            }

            return false;
        }
    }
}