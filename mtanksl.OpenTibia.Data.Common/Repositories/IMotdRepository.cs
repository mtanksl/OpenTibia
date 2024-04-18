using OpenTibia.Data.Models;

namespace OpenTibia.Data.Repositories
{
    public interface IMotdRepository
    {
        DbMotd GetLastMessageOfTheDay();
    }
}