using OpenTibia.Common.Objects;
using OpenTibia.Game.Common.ServerObjects;

namespace OpenTibia.Tests
{
    public class MockMessageCollectionFactory : IMessageCollectionFactory
    {
        public IMessageCollection Create()
        {
            return new MockMessageCollection();
        }

        public void Dispose()
        {
            
        }
    }
}