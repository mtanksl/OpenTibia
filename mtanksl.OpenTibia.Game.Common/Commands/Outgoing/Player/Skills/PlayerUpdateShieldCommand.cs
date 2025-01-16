using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateShieldCommand : Command
    {
        public PlayerUpdateShieldCommand(Player player, ulong shieldPoints, byte shield, byte shieldPercent)
        {
            Player = player;

            ShieldPoints = shieldPoints;

            Shield = shield;

            ShieldPercent = shieldPercent;
        }

        public Player Player { get; set; }

        public ulong ShieldPoints { get; set; }

        public byte Shield { get; set; }

        public byte ShieldPercent { get; set; }

        public override Promise Execute()
        {
            if (Player.Skills.ShieldPoints != ShieldPoints || Player.Skills.Shield != Shield || Player.Skills.ShieldPercent != ShieldPercent)
            {
                Player.Skills.ShieldPoints = ShieldPoints;

                if (Player.Skills.Shield != Shield || Player.Skills.ShieldPercent != ShieldPercent)
                {
                    Player.Skills.Shield = Shield;

                    Player.Skills.ShieldPercent = ShieldPercent;

                    Context.AddPacket(Player, new SendSkillsOutgoingPacket(Player.Skills.Fist, Player.Skills.FistPercent, Player.Skills.Club, Player.Skills.ClubPercent, Player.Skills.Sword, Player.Skills.SwordPercent, Player.Skills.Axe, Player.Skills.AxePercent, Player.Skills.Distance, Player.Skills.DistancePercent, Player.Skills.Shield, Player.Skills.ShieldPercent, Player.Skills.Fish, Player.Skills.FishPercent) );
                }

                Context.AddEvent(new PlayerUpdateShieldEventArgs(Player, ShieldPoints, Shield) );
            }

            return Promise.Completed;
        }
    }
}