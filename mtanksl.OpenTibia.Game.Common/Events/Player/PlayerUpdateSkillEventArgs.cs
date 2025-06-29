using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Events
{
    public class PlayerUpdateSkillEventArgs : GameEventArgs
    {
        public PlayerUpdateSkillEventArgs(Player player, Skill skill, ulong skillPoints, ushort skillLevel, byte skillPercent, int conditionSkillLevel, int itemSkillLevel)
        {
            Player = player;

            Skill = skill;

            SkillPoints = skillPoints;

            SkillLevel = skillLevel;

            SkillPercent = skillPercent;

            ConditionSkillLevel = conditionSkillLevel;

            ItemSkillLevel = itemSkillLevel;
        }

        public Player Player { get; }

        public Skill Skill { get; set; }

        public ulong SkillPoints { get; }

        public ushort SkillLevel { get; }

        public byte SkillPercent { get; }

        public int ConditionSkillLevel { get; }

        public int ItemSkillLevel { get; }
    }
}