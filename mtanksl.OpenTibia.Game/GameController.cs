using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game
{
    [Port(7172)]
    public class GameController : Controller
    {
        private Server server;

        public GameController(Server server)
        {
            this.server = server;
        }

        [Packet(0x0A)]
        public IActionResult Login(Login packet)
        {
            Connection.Keys = packet.Keys;

            if (packet.Version != 860)
            {
                Response.Write(new OpenSorryDialog(true, "Only protocol 8.6 allowed.") );

                return FlushAndClose();
            }

            var account = new Data.PlayerRepository().GetPlayer(packet.Account, packet.Password, packet.Character);

            if (account == null)
            {
                Response.Write(new OpenSorryDialog(true, "Account name or password is not correct.") );

                return FlushAndClose();
            }

            Connection.Client = new Client()
            {
                Player = new Player()
                {
                    Name = account.Name
                }
            };

            //TODO

            return Flush();
        }
    }
}