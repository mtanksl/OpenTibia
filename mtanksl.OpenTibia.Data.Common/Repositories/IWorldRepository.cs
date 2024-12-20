using OpenTibia.Data.Models;
using System.Threading.Tasks;

namespace OpenTibia.Data.Repositories
{
    public interface IWorldRepository
    {
        Task<DbWorld[]> GetWorlds();

        Task<DbWorld> GetWorldByName(string name);
    }
}