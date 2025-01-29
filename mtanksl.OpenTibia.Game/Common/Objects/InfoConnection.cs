using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.IO;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Sockets;
using System.Collections.Generic;
using System.Net.Sockets;

namespace OpenTibia.Common
{
    public class InfoConnection : RateLimitingConnection
    {
        private static Dictionary<byte, IPacketToCommand> firstCommands = new Dictionary<byte, IPacketToCommand>();

        static InfoConnection()
        {
            firstCommands.Add(0xFF, new PacketToCommand<InfoIncomingPacket>("Info", (connection, packet) => new ParseInfoProtocolCommand(connection, packet) ) );
        }

        private IServer server;

        public InfoConnection(IServer server, Socket socket) : base(server, socket)
        {
            this.server = server;

            MessageProtocol = MessageProtocol.Raw;
        }

        protected override void OnConnected()
        {
            server.Logger.WriteLine("Connected on info server", LogLevel.Debug);

            base.OnConnected();
        }

        private bool first = true;

        protected override void OnReceived(byte[] body, int length)
        {
            ByteArrayArrayStream stream = new ByteArrayArrayStream(body);

            ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);

            try
            {
                byte identification = reader.ReadByte();

                if (first)
                {
                    first = false;

                    if (firstCommands.TryGetValue(identification, out IPacketToCommand packetToCommand) )
                    {
                        Command command = packetToCommand.Convert(this, reader);

                        server.Logger.WriteLine("Received on info server: 0x" + identification.ToString("X2") + " (" + packetToCommand.Name + ")", LogLevel.Debug);

                        server.QueueForExecution( () =>
                        {
                            return Context.Current.AddCommand(command);
                        } );
                    }
                    else
                    {
                        IncreaseUnknownPacket();

                        server.Logger.WriteLine("Unknown packet received on info server: 0x" + identification.ToString("X2"), LogLevel.Warning);
                        
                        server.Logger.WriteLine(body.Print(0, length), LogLevel.Warning);

                        Disconnect();
                    }
                }
                else
                {
                    IncreaseInvalidMessage();

                    server.Logger.WriteLine("Invalid message received on info server", LogLevel.Warning);

                    server.Logger.WriteLine(body.Print(0, length), LogLevel.Warning);

                    Disconnect();
                }
            }
            catch
            {
                IncreaseInvalidMessage();

                server.Logger.WriteLine("Invalid message received on info server", LogLevel.Warning);

                server.Logger.WriteLine(body.Print(0, length), LogLevel.Warning);

                Disconnect();
            }

            base.OnReceived(body, length);
        }

        protected override void OnDisconnected(DisconnectedEventArgs e)
        {
            server.Logger.WriteLine("Disconnected on info server", LogLevel.Debug);
                        
            base.OnDisconnected(e);
        }
    }
}