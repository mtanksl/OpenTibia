using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Linq;

namespace OpenTibia.Game.Strategies
{
    public class RandomChooseTargetStrategy : IChooseTargetStrategy
    {
        public Player GetNext(Server server, Creature attacker)
        {
            Player target = server.GameObjects.GetPlayers()
                .Where(p => p.Vocation != Vocation.Gamemaster &&
                            attacker.Tile.Position.CanHearSay(p.Tile.Position) )
                .FirstOrDefault();

            return target;
        }
    }
}