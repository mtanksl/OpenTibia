using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class WatchScript : IItemUseScript
    {
        private HashSet<ushort> watches = new HashSet<ushort>() { 2036 };

        public void Start(Server server)
        {
            foreach (var openTibiaId in watches)
            {
                server.Scripts.ItemUseScripts.Add(openTibiaId, this);
            }
        }

        public void Stop(Server server)
        {

        }

        public bool OnItemUse(Player player, Item fromItem, Server server, Context context)
        {
            context.Write(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "The time is " + server.Clock.Hour.ToString("00") + ":" + server.Clock.Minute.ToString("00") + ".") );

            return true;
        }
    }
}