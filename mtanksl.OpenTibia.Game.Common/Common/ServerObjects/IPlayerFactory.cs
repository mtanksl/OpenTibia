using OpenTibia.Common.Objects;
using OpenTibia.Data.Models;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IPlayerFactory
    {
        Player Create(int databasePlayerId, int databaseAccountId, string name, Tile town, Tile spawn);

        void Attach(Player player);

        bool Detach(Player player);

        void ClearComponentsAndEventHandlers(Player player);

        void Load(DbPlayer dbPlayer, Player player);

        void Save(DbPlayer dbPlayer, Player player);
    }
}