using OpenTibia.Data.Models;

namespace OpenTibia.Data.Repositories
{
    public interface IPlayerRepository
    {
        DbAccount GetAccount(string accountName, string accountPassword);

        DbPlayer GetAccountPlayer(string accountName, string accountPassword, string playerName);

        DbPlayer[] GetPlayerByIds(int[] ids);

        DbPlayer GetPlayerByName(string name);
    }
}