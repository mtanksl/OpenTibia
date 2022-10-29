using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using OpenTibia.Game.Strategies;

namespace OpenTibia.Game
{
    public class PlayerFactory
    {
        private Server server;

        public PlayerFactory(Server server)
        {
            this.server = server;
        }

        public Player Create()
        {
            Player player = new Player();

            server.GameObjects.AddGameObject(player);

            server.Components.AddComponent(player, new AttackAndFollowBehaviour(new CloseAttackStrategy(500, (attacker, target) => -server.Randomization.Take(0, 20) ), new FollowWalkStrategy() ) );

            return player;
        }

        public void Destroy(Player player)
        {
            server.CancelQueueForExecution(Constants.PlayerWalkSchedulerEvent(player) );

            server.CancelQueueForExecution(Constants.PlayerAutomationSchedulerEvent(player) );

            server.GameObjects.RemoveGameObject(player);

            server.Components.ClearComponents(player);
        }
    }
}