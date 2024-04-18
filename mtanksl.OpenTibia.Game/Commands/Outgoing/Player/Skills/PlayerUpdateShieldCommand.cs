using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateShieldCommand : Command
    {
        public PlayerUpdateShieldCommand(Player player, byte shield, byte shieldPercent)
        {
            Player = player;

            Shield = shield;

            ShieldPercent = shieldPercent;
        }

        public Player Player { get; set; }

        public byte Shield { get; set; }

        public byte ShieldPercent { get; set; }

        public override Promise Execute()
        {
            if (Player.Skills.Shield != Shield || Player.Skills.FishPercent != ShieldPercent)
            {
                Player.Skills.Shield = Shield;

                Player.Skills.ShieldPercent = ShieldPercent;

                Context.AddPacket(Player, new SendSkillsOutgoingPacket(Player.Skills.Fist, Player.Skills.FistPercent, Player.Skills.Club, Player.Skills.ClubPercent, Player.Skills.Sword, Player.Skills.SwordPercent, Player.Skills.Axe, Player.Skills.AxePercent, Player.Skills.Distance, Player.Skills.DistancePercent, Player.Skills.Shield, Player.Skills.ShieldPercent, Player.Skills.Fish, Player.Skills.FishPercent) );
            }

            return Promise.Completed;
        }
    }
}