using System.Net.Sockets;

namespace OpenTibia
{
    public class TibiaLoginClientFactory : IClientFactory
    {
        public IClient Create(Socket socket)
        {
            return new TibiaLoginClient(socket);
        }
    }
}