using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateClubCommand : Command
    {
        public PlayerUpdateClubCommand(Player player, ulong clubPoints, byte club, byte clubPercent)
        {
            Player = player;

            ClubPoints = clubPoints;

            Club = club;

            ClubPercent = clubPercent;
        }

        public Player Player { get; set; }

        public ulong ClubPoints { get; set; }

        public byte Club { get; set; }

        public byte ClubPercent { get; set; }

        public override Promise Execute()
        {
            if (Player.Skills.ClubPoints != ClubPoints || Player.Skills.Club != Club || Player.Skills.ClubPercent != ClubPercent)
            {
                Player.Skills.ClubPoints = ClubPoints;

                if (Player.Skills.Club != Club || Player.Skills.ClubPercent != ClubPercent)
                {
                    Player.Skills.Club = Club;

                    Player.Skills.ClubPercent = ClubPercent;

                    Context.AddPacket(Player, new SendSkillsOutgoingPacket(Player.Skills.Fist, Player.Skills.FistPercent, Player.Skills.Club, Player.Skills.ClubPercent, Player.Skills.Sword, Player.Skills.SwordPercent, Player.Skills.Axe, Player.Skills.AxePercent, Player.Skills.Distance, Player.Skills.DistancePercent, Player.Skills.Shield, Player.Skills.ShieldPercent, Player.Skills.Fish, Player.Skills.FishPercent) );
                }

                Context.AddEvent(new PlayerUpdateClubEventArgs(Player, ClubPoints, Club) );
            }

            return Promise.Completed;
        }
    }
}