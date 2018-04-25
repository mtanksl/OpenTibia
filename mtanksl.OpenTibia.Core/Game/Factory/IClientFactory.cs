using System.Net.Sockets;

namespace OpenTibia
{
    public interface IClientFactory
    {
        IClient Create(Socket socket);
    }
}