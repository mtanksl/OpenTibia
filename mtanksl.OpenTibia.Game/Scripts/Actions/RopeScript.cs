using OpenTibia.Common.Objects;
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
            if (ropeSpots.Contains(toItem.Metadata.OpenTibiaId) )
            {


                return true;
            }
            
            return false;
        }
    }
}
