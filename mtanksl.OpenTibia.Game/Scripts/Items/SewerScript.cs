using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class SewerScript : ItemUseScript
    {
        private static HashSet<ushort> sewers = new HashSet<ushort>() { 430 };

        public override void Register(Server server)
        {
            foreach (var openTibiaId in sewers)
            {
                server.ItemUseScripts.Add(openTibiaId, this);
            }
        }

        public override bool Execute(Player player, Item fromItem, Server server, CommandContext context)
        {
            Tile toTile = server.Map.GetTile( ( (Tile)fromItem.Container ).Position.Offset(0, 0, 1) );

            if (toTile != null)
            {
                server.CancelQueueForExecution(Constants.PlayerSchedulerEvent(player) );

                new CreatureMoveCommand(player, toTile).Execute(server, context);

                return true;
            }

            return false;
        }
    }
}