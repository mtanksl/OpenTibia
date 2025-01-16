using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Events
{
    public class PlayerAdvanceSkillEventArgs : GameEventArgs
    {
        public PlayerAdvanceSkillEventArgs(Player player, Skill skill, byte fromLevel, byte toLevel)
        {
            Player = player;

            Skill = skill;

            FromLevel = fromLevel;

            ToLevel = toLevel;
        }

        public Player Player { get; }

        public Skill Skill { get; }

        public byte FromLevel { get; }

        public byte ToLevel { get; }
    }
}