using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateDistanceCommand : Command
    {
        public PlayerUpdateDistanceCommand(Player player, ulong distancePoints, byte distance, byte distancePercent)
        {
            Player = player;

            DistancePoints = distancePoints;

            Distance = distance;

            DistancePercent = distancePercent;
        }

        public Player Player { get; set; }

        public ulong DistancePoints { get; set; }

        public byte Distance { get; set; }

        public byte DistancePercent { get; set; }

        public override Promise Execute()
        {
            if (Player.Skills.DistancePoints != DistancePoints || Player.Skills.Distance != Distance || Player.Skills.DistancePercent != DistancePercent)
            {
                Player.Skills.DistancePoints = DistancePoints;

                if (Player.Skills.Distance != Distance || Player.Skills.DistancePercent != DistancePercent)
                {
                    Player.Skills.Distance = Distance;

                    Player.Skills.DistancePercent = DistancePercent;

                    Context.AddPacket(Player, new SendSkillsOutgoingPacket(Player.Skills.Fist, Player.Skills.FistPercent, Player.Skills.Club, Player.Skills.ClubPercent, Player.Skills.Sword, Player.Skills.SwordPercent, Player.Skills.Axe, Player.Skills.AxePercent, Player.Skills.Distance, Player.Skills.DistancePercent, Player.Skills.Shield, Player.Skills.ShieldPercent, Player.Skills.Fish, Player.Skills.FishPercent) );
                }

                Context.AddEvent(new PlayerUpdateDistanceEventArgs(Player, DistancePoints, Distance) );
            }

            return Promise.Completed;
        }
    }
}