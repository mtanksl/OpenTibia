using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class LadderScript : ItemUseScript
    {
        private static HashSet<ushort> ladders = new HashSet<ushort>() { 1386, 3678, 5543 };

        public override void Register(Server server)
        {
            foreach (var openTibiaId in ladders)
            {
                server.ItemUseScripts.Add(openTibiaId, this);
            }
        }

        public override bool Execute(Player player, Item fromItem, Server server, CommandContext context)
        {
            Tile toTile = server.Map.GetTile( ( (Tile)fromItem.Container ).Position.Offset(0, 1, -1) );

            if (toTile != null)
            {
                new CreatureMoveCommand(player, toTile).Execute(server, context);

                return true;
            }

            return false;
        }
    }
}