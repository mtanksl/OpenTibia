using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateFistCommand : Command
    {
        public PlayerUpdateFistCommand(Player player, byte fist, byte fistPercent)
        {
            Player = player;

            Fist = fist;

            FistPercent = fistPercent;
        }

        public Player Player { get; set; }

        public byte Fist { get; set; }

        public byte FistPercent { get; set; }

        public override Promise Execute()
        {
            if (Player.Skills.Fist != Fist || Player.Skills.FistPercent != FistPercent)
            {
                Player.Skills.Fist = Fist;

                Player.Skills.FistPercent = FistPercent;

                Context.AddPacket(Player.Client.Connection, new SendSkillsOutgoingPacket(Player.Skills.Fist, Player.Skills.FistPercent, Player.Skills.Club, Player.Skills.ClubPercent, player.Skills.Sword, player.Skills.SwordPercent, player.Skills.Axe, Player.Skills.AxePercent, Player.Skills.Distance, Player.Skills.DistancePercent, Player.Skills.Shield, Player.Skills.ShieldPercent, Player.Skills.Fish, Player.Skills.FishPercent));
            }

            return Promise.Completed;
        }
    }
}