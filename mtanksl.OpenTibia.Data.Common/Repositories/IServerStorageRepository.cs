using OpenTibia.Data.Models;
using System.Threading.Tasks;

namespace OpenTibia.Data.Repositories
{
    public interface IServerStorageRepository
    {
        Task<DbServerStorage[]> GetServerStorages();

        Task<DbServerStorage> GetServerStorageByKey(string key);

        void AddServerStorage(DbServerStorage serverStorage);
    }   
}