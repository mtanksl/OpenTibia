using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    [Port(7171)]
    public class LoginController : Controller
    {
        private Server server;

        public LoginController(Server server)
        {
            this.server = server;
        }

        [Packet(0x01)]
        public IActionResult EnterGame(EnterGame packet)
        {
            Connection.Keys = packet.Keys;

            if (packet.TibiaDat != 1277983123 || packet.TibiaPic != 1256571859 || packet.TibiaSpr != 1277298068 || packet.Version != 860)
            {
                Response.Write(new OpenSorryDialog(true, "Only protocol 8.6 allowed.") );

                return FlushAndClose();
            }
            
            var account = new Data.PlayerRepository().GetAccount(packet.Account, packet.Password);

            if (account == null)
            {
                Response.Write(new OpenSorryDialog(true, "Account name or password is not correct.") );

                return FlushAndClose();
            }

            List<Character> characters = new List<Character>();

            foreach (var player in account.Players)
            {
                characters.Add( new Character(player.Name, player.World.Name, player.World.Ip, (ushort)player.World.Port) );
            }

            Response.Write(new OpenSelectCharacterDialog(characters, (ushort)account.PremiumDays) );
            
            return FlushAndClose();
        }
    }
}