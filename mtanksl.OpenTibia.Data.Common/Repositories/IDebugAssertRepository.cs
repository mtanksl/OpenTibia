using OpenTibia.Data.Models;

namespace OpenTibia.Data.Repositories
{
    public interface IDebugAssertRepository
    {
        void AddDebugAssert(DbDebugAssert debugAssert);
    }
}