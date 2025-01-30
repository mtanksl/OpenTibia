using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IMessageCollectionFactory
    {
        IMessageCollection Create();
    }
}