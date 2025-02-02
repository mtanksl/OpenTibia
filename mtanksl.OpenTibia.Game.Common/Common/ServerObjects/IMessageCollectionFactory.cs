using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IMessageCollectionFactory : IDisposable
    {
        IMessageCollection Create();
    }
}