using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Mvc;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Controllers
{
    [Port(7172)]
    public class GameController : Controller
    {
        private Server server;

        public GameController(Server server, IConnection connection) : base(connection)
        {
            this.server = server;
        }

        [Packet(0x0A)]
        public void Login(Login packet)
        {
            Context.Request.Connection.Keys = packet.Keys;

            if (packet.Version != 860)
            {
                Context.Response.Write(Context.Request.Connection, new OpenSorryDialog(true, "Only protocol 8.6 allowed.") );

                Context.Response.Flush();

                return;
            }

            var account = new Data.PlayerRepository().GetPlayer(packet.Account, packet.Password, packet.Character);

            if (account == null)
            {
                Context.Response.Write(Context.Request.Connection, new OpenSorryDialog(true, "Account name or password is not correct.") );

                Context.Response.Flush();

                return;
            }

            Context.Request.Connection.Client = new Client(server)
            {
                Player = new Player()
                {
                    Name = account.Name
                }
            };

            server.CommandBus.Execute(new LoginCommand(server)
            {
                Player = Context.Request.Connection.Client.Player,

                FromPosition = new Position(account.CoordinateX, account.CoordinateY, account.CoordinateZ)

            }, Context);
        }

        [Packet(0x64)]
        public void WalkTo(WalkTo packet)
        {
            server.CommandBus.Execute(new WalkToCommand(server)
            {
                Player = Context.Request.Connection.Client.Player,

                MoveDirections = packet.MoveDirections

            }, Context);
        }

        [Packet(0x65)]
        public void WalkNorth()
        {
            server.CommandBus.Execute(new WalkCommand(server)
            {
                Player = Context.Request.Connection.Client.Player,

                MoveDirection = MoveDirection.North

            }, Context);
        }

        [Packet(0x66)]
        public void WalkEast()
        {
            server.CommandBus.Execute(new WalkCommand(server)
            {
                Player = Context.Request.Connection.Client.Player,

                MoveDirection = MoveDirection.East

            }, Context);
        }

        [Packet(0x67)]
        public void WalkSouth()
        {
            server.CommandBus.Execute(new WalkCommand(server)
            {
                Player = Context.Request.Connection.Client.Player,

                MoveDirection = MoveDirection.South

            }, Context);
        }

        [Packet(0x68)]
        public void WalkWest()
        {
            server.CommandBus.Execute(new WalkCommand(server)
            {
                Player = Context.Request.Connection.Client.Player,

                MoveDirection = MoveDirection.West

            }, Context);
        }

        [Packet(0x6A)]
        public void WalkNorthEast()
        {
            server.CommandBus.Execute(new WalkCommand(server)
            {
                Player = Context.Request.Connection.Client.Player,

                MoveDirection = MoveDirection.NorthEast

            }, Context);
        }

        [Packet(0x6B)]
        public void WalkSouthEast()
        {
            server.CommandBus.Execute(new WalkCommand(server)
            {
                Player = Context.Request.Connection.Client.Player,

                MoveDirection = MoveDirection.SouthEast

            }, Context);
        }

        [Packet(0x6C)]
        public void WalkSouthWest()
        {
            server.CommandBus.Execute(new WalkCommand(server)
            {
                Player = Context.Request.Connection.Client.Player,

                MoveDirection = MoveDirection.SouthWest

            }, Context);
        }

        [Packet(0x6D)]
        public void WalkNorthWest()
        {
            server.CommandBus.Execute(new WalkCommand(server)
            {
                Player = Context.Request.Connection.Client.Player,

                MoveDirection = MoveDirection.NorthWest

            }, Context);
        }
    }
}