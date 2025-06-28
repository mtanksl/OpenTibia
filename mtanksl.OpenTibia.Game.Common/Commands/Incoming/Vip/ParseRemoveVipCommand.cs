using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Incoming;

namespace OpenTibia.Game.Commands
{
    public class ParseRemoveVipCommand : IncomingCommand
    {
        public ParseRemoveVipCommand(Player player, RemoveVipIncomingPacket packet)
        {
            Player = player;

            Packet = packet;
        }

        public Player Player { get; set; }

        public RemoveVipIncomingPacket Packet { get; set; }

        public override Promise Execute()
        {
            Player.Vips.RemoveVip( (int)Packet.CreatureId);

            return Promise.Completed;
        }
    }
}