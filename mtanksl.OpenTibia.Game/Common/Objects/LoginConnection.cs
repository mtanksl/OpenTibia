using OpenTibia.Common.Events;
using OpenTibia.Game;
using OpenTibia.Game.Commands;
using OpenTibia.IO;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Sockets;
using OpenTibia.Security;
using System;
using System.Net.Sockets;

namespace OpenTibia.Common.Objects
{
    public class LoginConnection : Connection
    {
        private Server server;

        public LoginConnection(Server server, Socket socket) : base(socket)
        {
            this.server = server;
        }

        protected override void OnConnected()
        {
            server.Logger.WriteLine("Connected on login server", LogLevel.Debug);

            base.OnConnected();
        }

        protected override void OnReceived(byte[] body)
        {
            ByteArrayArrayStream stream = new ByteArrayArrayStream(body);

            ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);

            try
            {
                if (Adler32.Generate(body, 4) == reader.ReadUInt() )
                {
                    if (Keys == null)
                    {
                        Rsa.DecryptAndReplace(body, 21);
                    }
                    else
                    {

                    }

                    Command command = null;

                    byte identification = reader.ReadByte();

                    switch (identification)
                    {
                        case 0x01:
                            {
                                var packet = server.PacketsFactory.Create<EnterGameIncomingPacket>(reader);

                                command = new ParseEnterGameCommand(this, packet);
                            }
                            break;
                    }

                    if (command != null)
                    {
                        server.Logger.WriteLine("Received on login server: 0x" + identification.ToString("X2"), LogLevel.Debug);

                        server.QueueForExecution( () =>
                        {
                            Context context = Context.Current;

                            return context.AddCommand(command);
                        } );
                    }
                    else
                    {
                        server.Logger.WriteLine("Unknown packet received on login server: 0x" + identification.ToString("X2"), LogLevel.Warning);
                        
                        server.Logger.WriteLine(body.Print(), LogLevel.Warning);
                    }
                }
                else
                {
                    server.Logger.WriteLine("Invalid message received on login server.", LogLevel.Warning);

                    server.Logger.WriteLine(body.Print(), LogLevel.Warning);
                }
            }
            catch (Exception ex)
            {
                server.Logger.WriteLine(ex.ToString(), LogLevel.Error);
            }

            base.OnReceived(body);
        }

        protected override void OnDisconnected(DisconnectedEventArgs e)
        {
            server.Logger.WriteLine("Disconnected on login server", LogLevel.Debug);

            base.OnDisconnected(e);
        }
    }
}