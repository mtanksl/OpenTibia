using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseAddVipCommand : Command
    {
        public ParseAddVipCommand(Player player, string name)
        {
            Player = player;

            Name = name;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public override Promise Execute()
        {
            Player observer = Context.Server.GameObjects.GetPlayers()
                .Where(p => p.Name == Name)
                .FirstOrDefault();

            if (observer != null && observer != Player)
            {
                Vip vip = Player.Client.Vips.AddVip(observer.Name);

                Context.AddPacket(Player.Client.Connection, new VipOutgoingPacket(vip.Id, vip.Name, false) );

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}