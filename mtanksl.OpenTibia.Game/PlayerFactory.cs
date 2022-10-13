using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game
{
    public class PlayerFactory
    {
        private Server server;

        public PlayerFactory(Server server)
        {
            this.server = server;
        }

        public Player Create(string name)
        {
            Player player = new Player()
            {
                Name = name
            };

            server.GameObjects.AddGameObject(player);

            return player;
        }

        public void Destroy(Player player)
        {
            server.CancelQueueForExecution(Constants.PlayerWalkSchedulerEvent(player) );

            server.CancelQueueForExecution(Constants.PlayerAutomationSchedulerEvent(player) );

            server.GameObjects.RemoveGameObject(player);
        }
    }
}