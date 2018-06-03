using OpenTibia.Common.Events;
using OpenTibia.Game.Commands;
using OpenTibia.IO;
using OpenTibia.Mvc;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Network.Sockets;
using OpenTibia.Security;
using OpenTibia.Web;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace OpenTibia.Game.Connections
{
    public class GameConnection : Connection
    {
        private Server server;

        private int port;

        public GameConnection(Server server, int port, Socket socket) : base(socket)
        {
            this.server = server;

            this.port = port;
        }

        protected override void OnConnect()
        {
            Send( new Message() { new SendConnectionInfo() }.GetBytes(Keys) );

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
                        Rsa.DecryptAndReplace(body, 9);
                    }
                    else
                    {
                        Xtea.DecryptAndReplace(body, 4, 32, Keys);

                        stream.Seek(Origin.Current, 2);
                    }

                    ControllerMetadata controllerBaseMetadata = server.ControllerBaseMetadataFactory.Get(port, reader.ReadByte() );

                    if (controllerBaseMetadata != null)
                    {
                        Controller controller = (Controller)Activator.CreateInstance(controllerBaseMetadata.Type, new object[] { server, this } );

                        List<IIncomingPacket> parameters = new List<IIncomingPacket>();

                        foreach (var parameterType in controllerBaseMetadata.ParameterTypes)
                        {
                            IIncomingPacket packet = (IIncomingPacket)Activator.CreateInstance(parameterType);

                            packet.Read(reader);

                            parameters.Add(packet);
                        }

                        IActionResult result = (IActionResult)controllerBaseMetadata.Method.Invoke(controller, parameters.ToArray() );

                        if (result != null)
                        {
                            result.Execute(controller.Context);
                        }
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
            if (Client != null)
            {
                if (Client.Player != null)
                {
                    server.QueueForExecution(new Context(this), new CleanUpCommand(server) { Player = Client.Player }, null);
                }
            }
            
            base.OnDisconnected(e);
        }
    }
}