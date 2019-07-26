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
            TeleportCommand command = new TeleportCommand(player, ( (Tile)fromItem.Container ).Position.Offset(0, 0, 1) );

            command.Execute(server, context);

            return true;
        }
    }
}