using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class MailboxScript : IItemMoveScript
    {
        private HashSet<ushort> mailboxes = new HashSet<ushort>() { 2593 };

        private HashSet<Position> positions = new HashSet<Position>();

        public void Start(Server server)
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

            server.Scripts.ItemMoveScripts.Add(this);
        }

        public void Stop(Server server)
        {

        }

        public bool OnItemMove(Player player, Item fromItem, IContainer toContainer, byte toIndex, byte count, Server server, Context context)
        {
            if (toContainer is Tile toTile && positions.Contains(toTile.Position) )
            {

            }

            return false;
        }        
    }
}