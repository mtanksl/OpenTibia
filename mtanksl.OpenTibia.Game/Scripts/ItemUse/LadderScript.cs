using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class LadderScript : IItemUseScript
    {
        private HashSet<ushort> ladders = new HashSet<ushort>() { 1386, 3678, 5543 };

        public void Start(Server server)
        {
            foreach (var openTibiaId in ladders)
            {
                server.Scripts.ItemUseScripts.Add(openTibiaId, this);
            }
        }

        public void Stop(Server server)
        {

        }

        public bool OnItemUse(Player player, Item fromItem, Context context)
        {
            Tile toTile = context.Server.Map.GetTile( ( (Tile)fromItem.Container ).Position.Offset(0, 1, -1) );

            if (toTile != null)
            {
                new CreatureMoveCommand(player, toTile).Execute(context);

                return true;
            }

            return false;
        }
    }
}