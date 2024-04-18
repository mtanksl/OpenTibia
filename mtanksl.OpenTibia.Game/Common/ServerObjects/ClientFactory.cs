using OpenTibia.Common;
using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class ClientFactory : IClientFactory
    {
        private IServer server;

        public ClientFactory(IServer server)
        {
            this.server = server;
        }

        public IClient Create()
        {
            return new Client(server);
        }
    }
}