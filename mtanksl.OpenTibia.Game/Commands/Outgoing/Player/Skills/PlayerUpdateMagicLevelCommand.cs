using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateMagicLevelCommand : Command
    {
        public PlayerUpdateMagicLevelCommand(Player player, byte magicLevel, byte magicLevelPercent)
        {
            Player = player;

            MagicLevel = magicLevel;

            MagicLevelPercent = magicLevelPercent;
        }

        public Player Player { get; set; }

        public byte MagicLevel { get; set; }

        public byte MagicLevelPercent { get; set; }

        public override Promise Execute()
        {
            if (Player.Skills.MagicLevel != MagicLevel || Player.Skills.MagicLevelPercent != MagicLevelPercent)
            {
                Player.Skills.MagicLevel = MagicLevel;

                Player.Skills.MagicLevelPercent = MagicLevelPercent;

                Context.AddPacket(Player, new SendSkillsOutgoingPacket(Player.Skills.Fist, Player.Skills.FistPercent, Player.Skills.Club, Player.Skills.ClubPercent, Player.Skills.Sword, Player.Skills.SwordPercent, Player.Skills.Axe, Player.Skills.AxePercent, Player.Skills.Distance, Player.Skills.DistancePercent, Player.Skills.Shield, Player.Skills.ShieldPercent, Player.Skills.Fish, Player.Skills.FishPercent) );
            }

            return Promise.Completed;
        }
    }
}