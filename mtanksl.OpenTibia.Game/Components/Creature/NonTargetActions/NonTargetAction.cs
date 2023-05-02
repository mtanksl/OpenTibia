using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public abstract class NonTargetAction
    {
        public abstract Promise Update(Creature creature);
    }
}