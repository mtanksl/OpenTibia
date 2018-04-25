using System.Net.Sockets;

namespace OpenTibia
{
    public class TibiaGameClientFactory : IClientFactory
    {
        public IClient Create(Socket socket)
        {
            return new TibiaGameClient(socket);
        }
    }
}