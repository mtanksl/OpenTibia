using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class RopeScript : IItemUseWithItemScript
    {
        private HashSet<ushort> ropes = new HashSet<ushort>() { 2120 };

        private HashSet<ushort> ropeSpots = new HashSet<ushort> { 384 };

        public void Start(Server server)
        {
            foreach (var openTibiaId in ropes)
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
            if (ropeSpots.Contains(toItem.Metadata.OpenTibiaId) )
            {
                Tile toTile = context.Server.Map.GetTile( ( (Tile)toItem.Container ).Position.Offset(0, 1, -1) );

                if (toTile != null)
                {
                    new CreatureMoveCommand(player, toTile).Execute(context);

                    return true;
                }
            }   

            return false;
        }
    }
}