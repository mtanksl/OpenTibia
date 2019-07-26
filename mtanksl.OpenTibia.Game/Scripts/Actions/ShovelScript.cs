using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class ShovelScript : ItemUseWithItemScript
    {
        private static HashSet<ushort> shovels = new HashSet<ushort>() { 2554 };

        private static HashSet<ushort> stonePiles = new HashSet<ushort> { 468 };

        public override void Register(Server server)
        {
            foreach (var openTibiaId in shovels)
            {
                server.ItemUseWithItemScripts.Add(openTibiaId, this);
            }
        }

        public override bool Execute(Player player, Item fromItem, Item toItem, Server server, CommandContext context)
        {
            if (stonePiles.Contains(toItem.Metadata.OpenTibiaId) )
            {


                return true;
            }
            
            return false;
        }
    }
}