using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class SewerScript : IItemUseScript
    {
        private HashSet<ushort> sewers = new HashSet<ushort>() { 430 };

        public void Start(Server server)
        {
            foreach (var openTibiaId in sewers)
            {
                server.Scripts.ItemUseScripts.Add(openTibiaId, this);
            }
        }

        public void Stop(Server server)
        {

        }

        public bool OnItemUse(Player player, Item fromItem, Context context)
        {
            Tile toTile = context.Server.Map.GetTile( ( (Tile)fromItem.Container ).Position.Offset(0, 0, 1) );

            if (toTile != null)
            {
                new CreatureMoveCommand(player, toTile).Execute(context);

                return true;
            }

            return false;
        }
    }
}