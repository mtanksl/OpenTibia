using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Mvc;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Controllers
{
    [Port(7171)]
    public class LoginController : Controller
    {
        private Server server;

        public LoginController(Server server, IConnection connection) : base(connection)
        {
            this.server = server;
        }

        [Packet(0x01)]
        public void EnterGame(EnterGame packet)
        {
            Context.Request.Connection.Keys = packet.Keys;

            if (packet.TibiaDat != 1277983123 || packet.TibiaPic != 1256571859 || packet.TibiaSpr != 1277298068 || packet.Version != 860)
            {
                Context.Response.Write(Context.Request.Connection, new OpenSorryDialog(true, "Only protocol 8.6 allowed.") );

                Context.Response.Flush();

                return;
            }
            
            var account = new Data.PlayerRepository().GetAccount(packet.Account, packet.Password);

            if (account == null)
            {
                Context.Response.Write(Context.Request.Connection, new OpenSorryDialog(true, "Account name or password is not correct.") );

                Context.Response.Flush();

                return;
            }

            List<Character> characters = new List<Character>();

            foreach (var player in account.Players)
            {
                characters.Add( new Character(player.Name, player.World.Name, player.World.Ip, (ushort)player.World.Port) );
            }

            Context.Response.Write(Context.Request.Connection, new OpenSelectCharacterDialog(characters, (ushort)account.PremiumDays) );

            Context.Response.Flush();
        }
    }
}