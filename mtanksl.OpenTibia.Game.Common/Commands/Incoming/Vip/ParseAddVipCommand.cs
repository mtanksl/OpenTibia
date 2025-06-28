using OpenTibia.Common.Objects;
using OpenTibia.Data.Models;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;
using static OpenTibia.Common.Objects.PlayerVipCollection;

namespace OpenTibia.Game.Commands
{
    public class ParseAddVipCommand : IncomingCommand
    {
        public ParseAddVipCommand(Player player, AddVipIncomingPacket packet)
        {
            Player = player;

            Packet = packet;
        }

        public Player Player { get; set; }

        public AddVipIncomingPacket Packet { get; set; }

        public override async Promise Execute()
        {
            int maxVips = Player.Premium ? Context.Server.Config.GameplayVipPremiumLimit : Context.Server.Config.GameplayVipFreeLimit;

            if (Player.Vips.Count < maxVips)
            {
                using (var database = Context.Server.DatabaseFactory.Create() )
                {
                    DbPlayer dbPlayer = await database.PlayerRepository.GetPlayerByName(Packet.Name);

                    if (dbPlayer != null && dbPlayer.Id != Player.DatabasePlayerId)
                    {
                        Vip vip = new Vip()
                        {
                            Name = dbPlayer.Name,

                            Description = null,

                            IconId = 10,

                            NotifyLogin = true
                        };

                        if (Player.Vips.AddVip(dbPlayer.Id, vip) )
                        {
                            Context.AddPacket(Player, new VipOutgoingPacket( (uint)dbPlayer.Id, vip.Name, vip.Description, vip.IconId, vip.NotifyLogin, Context.Server.GameObjects.GetPlayerByName(dbPlayer.Name) != null) );

                            await Promise.Completed; return;
                        }
                    }
                }
            }

            await Promise.Break; return;
        }
    }
}