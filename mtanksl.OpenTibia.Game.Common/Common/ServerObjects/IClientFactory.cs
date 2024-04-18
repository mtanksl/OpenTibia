using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IClientFactory
    {
        IClient Create();
    }
}