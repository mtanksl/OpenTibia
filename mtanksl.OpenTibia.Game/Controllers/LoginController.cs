using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Mvc;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Web;
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

        public IActionResult Command(Command command)
        {
            return new CommandResult(server, Context, command);
        }

        public IActionResult DelayedCommand(string key, int delay, Command command)
        {
            return new DelayedCommandResult(server, key, delay, Context, command);
        }

        [Packet(0x01)]
        public IActionResult EnterGame(EnterGame packet)
        {
            Context.Request.Connection.Keys = packet.Keys;

            if (packet.TibiaDat != 1277983123 || packet.TibiaPic != 1256571859 || packet.TibiaSpr != 1277298068 || packet.Version != 860)
            {
                Context.Response.Write(Context.Request.Connection, new Network.Packets.Outgoing.OpenSorryDialog(true, Constants.OnlyProtocol86Allowed) );

                Context.Response.Flush();

                return null;
            }
            
            var account = new Data.PlayerRepository().GetAccount(packet.Account, packet.Password);

            if (account == null)
            {
                Context.Response.Write(Context.Request.Connection, new Network.Packets.Outgoing.OpenSorryDialog(true, Constants.AccountNameOrPasswordIsNotCorrect) );

                Context.Response.Flush();

                return null;
            }

            List<Character> characters = new List<Character>();

            foreach (var player in account.Players)
            {
                characters.Add( new Character(player.Name, player.World.Name, player.World.Ip, (ushort)player.World.Port) );
            }

            Context.Response.Write(Context.Request.Connection, new Network.Packets.Outgoing.OpenSelectCharacterDialog(characters, (ushort)account.PremiumDays) );

            Context.Response.Flush();

            return null;
        }
    }
}