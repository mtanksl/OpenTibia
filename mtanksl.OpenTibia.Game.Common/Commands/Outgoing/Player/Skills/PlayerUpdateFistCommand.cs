using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateFistCommand : Command
    {
        public PlayerUpdateFistCommand(Player player, ulong fistPoints, byte fist, byte fistPercent)
        {
            Player = player;

            FistPoints = fistPoints;

            Fist = fist;

            FistPercent = fistPercent;
        }

        public Player Player { get; set; }

        public ulong FistPoints { get; set; }

        public byte Fist { get; set; }

        public byte FistPercent { get; set; }

        public override Promise Execute()
        {
            if (Player.Skills.FistPoints != FistPoints || Player.Skills.Fist != Fist || Player.Skills.FistPercent != FistPercent)
            {
                Player.Skills.FistPoints = FistPoints;

                if (Player.Skills.Fist != Fist || Player.Skills.FistPercent != FistPercent)
                {
                    Player.Skills.Fist = Fist;

                    Player.Skills.FistPercent = FistPercent;

                    Context.AddPacket(Player, new SendSkillsOutgoingPacket(Player.Skills.Fist, Player.Skills.FistPercent, Player.Skills.Club, Player.Skills.ClubPercent, Player.Skills.Sword, Player.Skills.SwordPercent, Player.Skills.Axe, Player.Skills.AxePercent, Player.Skills.Distance, Player.Skills.DistancePercent, Player.Skills.Shield, Player.Skills.ShieldPercent, Player.Skills.Fish, Player.Skills.FishPercent) );
                }

                Context.AddEvent(new PlayerUpdateFistEventArgs(Player, FistPoints, Fist) );
            }

            return Promise.Completed;
        }
    }
}