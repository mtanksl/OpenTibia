using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components
{
    public class DoNotWalkStrategy : IWalkStrategy
    {
        public static readonly DoNotWalkStrategy Instance = new DoNotWalkStrategy();

        private DoNotWalkStrategy()
        {
            
        }

        public bool CanWalk(Creature attacker, Creature target, out Tile tile)
        {
            tile = null;

            return false;
        }
    }
}