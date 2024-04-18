using OpenTibia.Game.Common;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerLoginVipHandler : EventHandler<PlayerLoginEventArgs>
    {
        public override Promise Handle(PlayerLoginEventArgs e)
        {
            foreach (var observer in Context.Server.GameObjects.GetPlayers() )
            {
                if (observer.Vips.TryGetVip(e.Player.DatabasePlayerId, out _) )
                {
                    Context.AddPacket(observer, new VipLoginOutgoingPacket( (uint)e.Player.DatabasePlayerId) );
                }
            }

            return Promise.Completed;
        }
    }
}