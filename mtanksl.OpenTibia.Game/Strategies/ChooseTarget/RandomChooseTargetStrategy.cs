using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Linq;

namespace OpenTibia.Game.Strategies
{
    public class RandomChooseTargetStrategy : IChooseTargetStrategy
    {
        private Player target;

        public Player GetNext(Server server, Creature attacker)
        {
            if (target == null || target.IsDestroyed || !attacker.Tile.Position.CanHearSay(target.Tile.Position) )
            {
                Player[] players = server.GameObjects.GetPlayers()
                    .Where(p => p.Vocation != Vocation.Gamemaster &&
                                attacker.Tile.Position.CanHearSay(p.Tile.Position) )
                    .ToArray();

                if (players.Length > 0)
                {
                    target = server.Randomization.Take(players);
                }
            }

            return target;
        }
    }
}