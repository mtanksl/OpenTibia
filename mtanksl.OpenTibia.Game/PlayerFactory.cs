using OpenTibia.Common.Objects;

namespace OpenTibia.Game
{
    public class PlayerFactory
    {
        private GameObjectCollection gameObjectCollection;

        public PlayerFactory(GameObjectCollection gameObjectCollection)
        {
            this.gameObjectCollection = gameObjectCollection;
        }

        public Player Create(string name)
        {
            Player player = new Player()
            {
                Name = name
            };

            gameObjectCollection.AddGameObject(player);

            return player;
        }

        public void Destroy(Player player)
        {
            gameObjectCollection.RemoveGameObject(player);
        }
    }
}