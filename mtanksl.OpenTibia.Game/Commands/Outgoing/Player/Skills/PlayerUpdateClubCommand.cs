using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateClubCommand : Command
    {
        public PlayerUpdateClubCommand(Player player, byte club, byte clubPercent)
        {
            Player = player;

            Club = club;

            ClubPercent = clubPercent;
        }

        public Player Player { get; set; }

        public byte Club { get; set; }

        public byte ClubPercent { get; set; }

        public override Promise Execute()
        {
            if (Player.Skills.Club != Club || Player.Skills.ClubPercent != ClubPercent)
            {
                Player.Skills.Club = Club;

                Player.Skills.ClubPercent = ClubPercent;

                Context.AddPacket(Player.Client.Connection, new SendSkillsOutgoingPacket(Player.Skills.Fist, Player.Skills.FistPercent, Player.Skills.Club, Player.Skills.ClubPercent, player.Skills.Sword, player.Skills.SwordPercent, player.Skills.Axe, Player.Skills.AxePercent, Player.Skills.Distance, Player.Skills.DistancePercent, Player.Skills.Shield, Player.Skills.ShieldPercent, Player.Skills.Fish, Player.Skills.FishPercent) );
            }

            return Promise.Completed;
        }
    }
}