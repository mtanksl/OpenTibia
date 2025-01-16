using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateFishCommand : Command
    {
        public PlayerUpdateFishCommand(Player player, ulong fishPoints, byte fish, byte fishPercent)
        {
            Player = player;

            FishPoints = fishPoints;

            Fish = fish;

            FishPercent = fishPercent;
        }

        public Player Player { get; set; }

        public ulong FishPoints { get; set; }

        public byte Fish { get; set; }

        public byte FishPercent { get; set; }

        public override Promise Execute()
        {
            if (Player.Skills.FishPoints != FishPoints || Player.Skills.Fish != Fish || Player.Skills.FishPercent != FishPercent)
            {
                Player.Skills.FishPoints = FishPoints;

                if (Player.Skills.Fish != Fish || Player.Skills.FishPercent != FishPercent)
                {
                    Player.Skills.Fish = Fish;

                    Player.Skills.FishPercent = FishPercent;

                    Context.AddPacket(Player, new SendSkillsOutgoingPacket(Player.Skills.Fist, Player.Skills.FistPercent, Player.Skills.Club, Player.Skills.ClubPercent, Player.Skills.Sword, Player.Skills.SwordPercent, Player.Skills.Axe, Player.Skills.AxePercent, Player.Skills.Distance, Player.Skills.DistancePercent, Player.Skills.Shield, Player.Skills.ShieldPercent, Player.Skills.Fish, Player.Skills.FishPercent) );
                }

                Context.AddEvent(new PlayerUpdateFishEventArgs(Player, FishPoints, Fish) );
            }

            return Promise.Completed;
        }
    }
}