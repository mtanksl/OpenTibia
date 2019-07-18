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

        public LoginConnection(Server server, int port, Socket socket) : base(port, socket)
        {
            this.server = server;
        }

        protected override void OnConnect()
        {
            base.OnConnect();
        }

        protected override void OnReceive(byte[] body)
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

                    Command command = null;

                    switch (reader.ReadByte() )
                    {
                        case 0x01:

                            command = new EnterGameCommand(this, server.PacketsFactory.Create<EnterGameIncomingPacket>(reader) );

                            break;
                    }

                    if (command != null)
                    {
                        server.QueueForExecution(command);
                    }
                }
            }
            catch (Exception ex)
            {
                server.Logger.WriteLine(ex.ToString() );
            }

            base.OnReceive(body);
        }

        protected override void OnDisconnected(DisconnectedEventArgs e)
        {
            base.OnDisconnected(e);
        }
    }
}