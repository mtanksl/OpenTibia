using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class PlayerAdvanceSkillPlugin : Plugin
    {
        public abstract Promise OnAdvanceSkill(Player player, Skill skill, byte fromLevel, byte toLevel);
    }
}