using OpenTibia.Game;
using OpenTibia.Game.Commands;
using OpenTibia.IO;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Sockets;
using OpenTibia.Security;
using System.Collections.Generic;
using System.Net.Sockets;

namespace OpenTibia.Common.Objects
{
    public class LoginConnection : RateLimitingConnection
    {
        private Server server;

        public LoginConnection(Server server, Socket socket) : base(server, socket)
        {
            this.server = server;

            firstCommands.Add(0x01, new PacketToCommand<EnterGameIncomingPacket>(packet => new ParseEnterGameCommand(this, packet) ) );
        }

        protected override void OnConnected()
        {
            server.Logger.WriteLine("Connected on login server", LogLevel.Debug);

            base.OnConnected();
        }

        private bool first = true;

        private Dictionary<byte, IPacketToCommand> firstCommands = new Dictionary<byte, IPacketToCommand>();

        protected override void OnReceived(byte[] body, int length)
        {
            ByteArrayArrayStream stream = new ByteArrayArrayStream(body);

            ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);

            try
            {
                if (Adler32.Generate(body, 4, length - 4) == reader.ReadUInt() )
                {
                    if (Keys == null)
                    {
                        Rsa.DecryptAndReplace(body, 21, length - 21);
                    }
                    else
                    {

                    }

                    byte identification = reader.ReadByte();

                    if (first)
                    {
                        first = false;

                        if (firstCommands.TryGetValue(identification, out var packetToCommand) )
                        {
                            Command command = packetToCommand.Convert(reader);

                            server.Logger.WriteLine("Received on login server: 0x" + identification.ToString("X2"), LogLevel.Debug);

                            server.QueueForExecution( () =>
                            {
                                return Context.Current.AddCommand(command);
                            } );
                        }
                        else
                        {
                            IncreaseUnknownPacket();

                            server.Logger.WriteLine("Unknown packet received on login server: 0x" + identification.ToString("X2"), LogLevel.Warning);
                        
                            server.Logger.WriteLine(body.Print(0, length), LogLevel.Warning);
                        }
                    }
                    else
                    {
                        Disconnect();
                    }
                }
                else
                {
                    IncreaseInvalidMessage();

                    server.Logger.WriteLine("Invalid message received on login server", LogLevel.Warning);

                    server.Logger.WriteLine(body.Print(0, length), LogLevel.Warning);
                }
            }
            catch
            {
                IncreaseInvalidMessage();

                server.Logger.WriteLine("Invalid message received on login server", LogLevel.Warning);

                server.Logger.WriteLine(body.Print(0, length), LogLevel.Warning);
            }

            base.OnReceived(body, length);
        }

        protected override void OnDisconnected(DisconnectedEventArgs e)
        {
            server.Logger.WriteLine("Disconnected on login server", LogLevel.Debug);
            
            base.OnDisconnected(e);
        }
    }
}