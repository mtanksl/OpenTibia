using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game
{
    public class PlayerFactory
    {
        private GameObjectCollection gameObjectCollection;

        public PlayerFactory(GameObjectCollection gameObjectCollection)
        {
            this.gameObjectCollection = gameObjectCollection;
        }

        public Player Create(string name, Action<Player> initialize = null)
        {
            Player player = new Player()
            {
                Name = name
            };

            if (initialize != null)
            {
                initialize(player);
            }

            gameObjectCollection.AddGameObject(player);

            return player;
        }

        public void Destroy(Player player)
        {
            gameObjectCollection.RemoveGameObject(player);
        }
    }
}