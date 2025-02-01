using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;

namespace OpenTibia.Tests
{
    public class Test
    {
        public static void Run(Action<Test> options, Action<Test> initialize = null, Action<Test> cleanup = null)
        {
            var t = new Test(MyTestContext.Server);

            try
            {
                if (initialize != null)
                {
                    initialize(t);
                }

                options(t);
            }
            finally
            {
                foreach (var player in t.Server.GameObjects.GetPlayers().ToArray() )
                {
                    t.Logout(player);
                }

                if (cleanup != null)
                {
                    cleanup(t);
                }
            }
        }

        private Test(IServer server)
        {
            this.server = server;
        }

        private IServer server;

        public IServer Server
        {
            get
            {
                return server;
            }
        }

        public Player Login(string account, string password, string character)
        {
            if (character == "Account Manager")
            {
                var builder = 

                    Using("127.0.0.1", a => a
                        .Execute(new ParseSelectedCharacterCommand(a.Connection, new SelectedCharacterIncomingPacket() { OperatingSystem = Common.Structures.OperatingSystem.Windows, Version = 860, Keys = new uint[] { 0, 0, 0, 0 }, Gamemaster = true, Account = account, Password = password, Character = character, Nonce = 0 } ) )
                        .ExpectSuccess()
                        .Observe(o => o
                            .ExpectPacket(9)
                            .ExpectPacket<SendInfoOutgoingPacket>(1)
                            .ExpectPacket<SendTilesOutgoingPacket>(1)
                            .ExpectPacket<SetEnvironmentLightOutgoingPacket>(1)
                            .ExpectPacket<SendStatusOutgoingPacket>(1)
                            .ExpectPacket<SendSkillsOutgoingPacket>(1)
                            .ExpectPacket<SetSpecialConditionOutgoingPacket>(1)
                            .ExpectPacket<ShowMagicEffectOutgoingPacket>(1)
                            .ExpectPacket<ShowWindowTextOutgoingPacket>(2)
                            .ExpectConnected() ) );

                builder.Run();

                return builder.Connection.Client.Player;
            }
            else
            {
                var builder = 

                    Using("127.0.0.1", a => a
                        .Execute(new ParseSelectedCharacterCommand(a.Connection, new SelectedCharacterIncomingPacket() { OperatingSystem = Common.Structures.OperatingSystem.Windows, Version = 860, Keys = new uint[] { 0, 0, 0, 0 }, Gamemaster = true, Account = account, Password = password, Character = character, Nonce = 0 } ) )
                        .ExpectSuccess()
                        .Observe(o => o
                            .ExpectPacket(9)
                            .ExpectPacket<SendInfoOutgoingPacket>(1)
                            .ExpectPacket<SendTilesOutgoingPacket>(1)
                            .ExpectPacket<SetEnvironmentLightOutgoingPacket>(1)
                            .ExpectPacket<SendStatusOutgoingPacket>(1)
                            .ExpectPacket<SendSkillsOutgoingPacket>(1)
                            .ExpectPacket<SetSpecialConditionOutgoingPacket>(1)
                            .ExpectPacket<SlotAddOutgoingPacket>(1)
                            .ExpectPacket<ShowMagicEffectOutgoingPacket>(1)
                            .ExpectPacket<ShowWindowTextOutgoingPacket>(1)
                            .ExpectConnected() ) );

                builder.Run();

                return builder.Connection.Client.Player;
            }
        }

        public void Move(Player player, Position position)
        {
            Using(player, a => a
                .Execute(new CreatureMoveCommand(a.Connection.Client.Player, server.Map.GetTile(position) ) )
                .ExpectSuccess() )
            .Run();
        }

        public void Logout(Player player)
        {
            Using(player, a => a
                .Execute(new ParseLogOutCommand(a.Connection.Client.Player) )
                .ExpectSuccess()
                .Observe(o => o
                    .ExpectPacket(1)
                    .ExpectPacket<ShowMagicEffectOutgoingPacket>(1)
                    .ExpectConnected(false) ) )
            .Run();
        } 

        public IActionBuilder Using(string ipAddress, Action<IActionBuilder> options)
        {
            return Using(new MockConnection(ipAddress), options);
        }

        public IActionBuilder Using(Player player, Action<IActionBuilder> options)
        {
            return Using(player.Client.Connection, options);
        }

        public IActionBuilder Using(IConnection connection, Action<IActionBuilder> options)
        {
            return new ActionBuilder(server, connection, options);
        }
    }
}