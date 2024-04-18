using OpenTibia.Common.Objects;
using OpenTibia.Data.Models;
using OpenTibia.Game.GameObjectScripts;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IPlayerFactory
    {
        void Start();

        GameObjectScript<string, Player> GetPlayerGameObjectScript(string name);

        Player Create(int databasePlayerId, string name, Tile town, Tile spawn);

        void Attach(Player player);

        bool Detach(Player player);

        void ClearComponentsAndEventHandlers(Player player);

        void Load(DbPlayer dbPlayer, Player player);

        void Save(DbPlayer dbPlayer, Player player);
    }
}