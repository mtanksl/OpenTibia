using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System.Linq;

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

            server.Components.AddComponent(player, new AttackAndFollowBehaviour() );

            return player;
        }

        public void Destroy(Player player)
        {
            server.CancelQueueForExecution(Constants.PlayerWalkSchedulerEvent(player) );

            server.CancelQueueForExecution(Constants.PlayerAutomationSchedulerEvent(player) );

            server.GameObjects.RemoveGameObject(player);

            foreach (var component in server.Components.GetComponents<Component>(player).ToList() )
            {
                server.Components.RemoveComponent(player, component);
            }
        }
    }
}