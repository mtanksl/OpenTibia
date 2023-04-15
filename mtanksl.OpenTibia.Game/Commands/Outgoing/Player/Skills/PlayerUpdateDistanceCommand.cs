using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateDistanceCommand : Command
    {
        public PlayerUpdateDistanceCommand(Player player, byte distance, byte distancePercent)
        {
            Player = player;

            Distance = distance;

            DistancePercent = distancePercent;
        }

        public Player Player { get; set; }

        public byte Distance { get; set; }

        public byte DistancePercent { get; set; }

        public override Promise Execute()
        {
            if (Player.Skills.Distance != Distance || Player.Skills.DistancePercent != DistancePercent)
            {
                Player.Skills.Distance = Distance;

                Player.Skills.DistancePercent = DistancePercent;

                Context.AddPacket(Player.Client.Connection, new SendSkillsOutgoingPacket(Player.Skills.Fist, Player.Skills.FistPercent, Player.Skills.Club, Player.Skills.ClubPercent, player.Skills.Sword, player.Skills.SwordPercent, player.Skills.Axe, Player.Skills.AxePercent, Player.Skills.Distance, Player.Skills.DistancePercent, Player.Skills.Shield, Player.Skills.ShieldPercent, Player.Skills.Fish, Player.Skills.FishPercent) );
            }

            return Promise.Completed;
        }
    }
}