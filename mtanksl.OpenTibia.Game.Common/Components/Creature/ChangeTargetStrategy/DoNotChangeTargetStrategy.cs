using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components
{
    public class DoNotChangeTargetStrategy : IChangeTargetStrategy
    {
        public static readonly DoNotChangeTargetStrategy Instance = new DoNotChangeTargetStrategy();

        private DoNotChangeTargetStrategy()
        {
            
        }

        public bool ShouldChange(int ticks, Creature attacker, Creature target)
        {
            return false;
        }
    }
}