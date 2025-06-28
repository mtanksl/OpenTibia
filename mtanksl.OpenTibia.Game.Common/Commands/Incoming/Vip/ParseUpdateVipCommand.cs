using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Incoming;
using static OpenTibia.Common.Objects.PlayerVipCollection;

namespace OpenTibia.Game.Commands
{
    public class ParseUpdateVipCommand : IncomingCommand
    {
        public ParseUpdateVipCommand(Player player, UpdateVipIncomingPacket packet)
        {
            Player = player;

            Packet = packet;
        }

        public Player Player { get; set; }

        public UpdateVipIncomingPacket Packet { get; set; }

        public override Promise Execute()
        {
            Vip vip;

            if (Player.Vips.TryGetVip((int)Packet.CreatureId, out vip) )
            {
                vip.Description = Packet.Description;

                vip.IconId = Packet.IconId;

                vip.NotifyLogin = Packet.NotifyLogin;
            }
            
            return Promise.Completed;
        }
    }
}