using OpenTibia.Data.Models;
using System.Threading.Tasks;

namespace OpenTibia.Data.Repositories
{
    public interface IMotdRepository
    {
        Task<DbMotd> GetLastMessageOfTheDay();

        void AddMessageOfTheDay(DbMotd motd);
    }
}