using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class FirstChooseTargetStrategy : IChooseTargetStrategy
    {
        public FirstChooseTargetStrategy()
        {
            
        }

        private Player target;

        public Player GetNext(Server server, Creature attacker)
        {
            if (target == null || target.IsDestroyed || !attacker.Tile.Position.CanHearSay(target.Tile.Position) )
            {
                Player player = server.GameObjects.GetPlayers()
                    .Where(p => p.Vocation != Vocation.Gamemaster &&
                                attacker.Tile.Position.CanHearSay(p.Tile.Position) )
                    .FirstOrDefault();

                target = player;
            }

            return target;
        }
    }
}