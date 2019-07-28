using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class MailboxScript : IItemMoveScript
    {
        private static HashSet<ushort> mailboxes = new HashSet<ushort>() { 2593 };

        public void Register(Server server)
        {
            foreach (var tile in server.Map.GetTiles() )
            {
                foreach (var item in tile.GetItems() )
                {
                    if (mailboxes.Contains(item.Metadata.OpenTibiaId) )
                    {
                        positions.Add(tile.Position);

                        break;
                    }
                }
            }

            server.ItemMoveScripts.Add(this);
        }

        private HashSet<Position> positions = new HashSet<Position>();

        public bool Execute(Player player, Item fromItem, IContainer toContainer, byte toIndex, byte count, Server server, CommandContext context)
        {
            if (toContainer is Tile toTile)
            {
                if ( positions.Contains(toTile.Position) )
                {
                    //TODO

                    return true;
                }
            }

            return false;
        }        
    }
}