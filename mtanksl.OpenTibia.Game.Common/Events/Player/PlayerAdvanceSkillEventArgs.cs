using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Events
{
    public class PlayerAdvanceSkillEventArgs : GameEventArgs
    {
        public PlayerAdvanceSkillEventArgs(Player player, Skill skill, ushort fromLevel, ushort toLevel)
        {
            Player = player;

            Skill = skill;

            FromLevel = fromLevel;

            ToLevel = toLevel;
        }

        public Player Player { get; }

        public Skill Skill { get; }

        public ushort FromLevel { get; }

        public ushort ToLevel { get; }
    }
}