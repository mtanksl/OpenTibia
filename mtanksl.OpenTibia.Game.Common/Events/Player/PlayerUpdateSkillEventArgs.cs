using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Events
{
    public class PlayerUpdateSkillEventArgs : GameEventArgs
    {
        public PlayerUpdateSkillEventArgs(Player player, Skill skill, ulong skillPoints, byte skillLevel, byte skillPercent)
        {
            Player = player;

            Skill = skill;

            SkillPoints = skillPoints;

            SkillLevel = skillLevel;

            SkillPercent = skillPercent;
        }

        public Player Player { get; }

        public Skill Skill { get; set; }

        public ulong SkillPoints { get; }

        public byte SkillLevel { get; }

        public byte SkillPercent { get; }
    }
}