using OpenTibia.IO;
using OpenTibia.Mvc;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Network.Sockets;
using OpenTibia.Security;
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
        }

        protected override void OnReceive(byte[] body)
        {
            ByteArrayArrayStream stream = new ByteArrayArrayStream(body);

            ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);

            if (Adler32.Generate(body, 4) == reader.ReadUInt())
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

                byte identifier = reader.ReadByte();

                ControllerMetadata controllerBaseMetadata = server.ControllerBaseMetadataFactory.Get(port, identifier);

                if (controllerBaseMetadata != null)
                {
                    Controller controller = (Controller)Activator.CreateInstance(controllerBaseMetadata.Type, new object[] { server, this } );

                    List<object> parameters = new List<object>();

                    foreach (var parameterType in controllerBaseMetadata.ParameterTypes)
                    {
                        IIncomingPacket packet = (IIncomingPacket)Activator.CreateInstance(parameterType);

                        packet.Read(reader);

                        parameters.Add(packet);
                    }

                    server.Dispatcher.QueueForExecution( () =>
                    {
                        controllerBaseMetadata.Method.Invoke(controller, parameters.ToArray() );
                    } );
                }
            }
        }
    }
}