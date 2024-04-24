using OpenTibia.Data.Models;

namespace OpenTibia.Data.Repositories
{
    public interface IWorldRepository
    {
        DbWorld[] GetWorlds();
    }
}