using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components
{
    public class DoChangeTargetStrategy : IChangeTargetStrategy
    {
        public static readonly DoChangeTargetStrategy Instance = new DoChangeTargetStrategy();

        private DoChangeTargetStrategy()
        {
            
        }

        public bool ShouldChange(int ticks, Creature attacker, Player target)
        {
            return true;
        }
    }
}