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

                    switch (reader.ReadByte() )
                    {
                        case 0x01:
                            {
                                var packet = server.PacketsFactory.Create<EnterGameIncomingPacket>(reader);

                                command = new EnterGameCommand(this, packet);
                            }
                            break;
                    }

                    if (command != null)
                    {
                        server.QueueForExecution(ctx =>
                        {
                            ctx.AddCommand(command);
                        } );
                    }
                }
            }
            catch (Exception ex)
            {
                server.Logger.WriteLine(ex.ToString() );
            }

            base.OnReceived(body);
        }

        protected override void OnDisconnected(DisconnectedEventArgs e)
        {
            base.OnDisconnected(e);
        }
    }
}