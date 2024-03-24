using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateAxeCommand : Command
    {
        public PlayerUpdateAxeCommand(Player player, byte axe, byte axePercent)
        {
            Player = player;

            Axe = axe;

            AxePercent = axePercent;
        }

        public Player Player { get; set; }

        public byte Axe { get; set; }

        public byte AxePercent { get; set; }

        public override Promise Execute()
        {
            if (Player.Skills.Axe != Axe || Player.Skills.AxePercent != AxePercent)
            {
                Player.Skills.Axe = Axe;

                Player.Skills.AxePercent = AxePercent;

                Context.AddPacket(Player, new SendSkillsOutgoingPacket(Player.Skills.Fist, Player.Skills.FistPercent, Player.Skills.Club, Player.Skills.ClubPercent, Player.Skills.Sword, Player.Skills.SwordPercent, Player.Skills.Axe, Player.Skills.AxePercent, Player.Skills.Distance, Player.Skills.DistancePercent, Player.Skills.Shield, Player.Skills.ShieldPercent, Player.Skills.Fish, Player.Skills.FishPercent) );
            }

            return Promise.Completed;
        }
    }
}