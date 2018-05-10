using OpenTibia.IO;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Security;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;

namespace OpenTibia.Game
{
    public class GameConnection : Connection
    {
        private Server server;

        public GameConnection(Server server, Socket socket) : base(socket)
        {
            this.server = server;
        }

        protected override void OnConnect()
        {
            Send( new Message() { new SendConnectionInfo() }.GetBytes(Keys) );
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

                    byte identifier = reader.ReadByte();

                    foreach (var type in Assembly.GetExecutingAssembly().GetTypes() )
                    {
                        PortAttribute portAttribute = type.GetCustomAttribute<PortAttribute>();

                        if (portAttribute != null && portAttribute.Port == 7172)
                        {
                            foreach (var method in type.GetMethods() )
                            {
                                PacketAttribute packetAttribute = method.GetCustomAttribute<PacketAttribute>();

                                if (packetAttribute != null && packetAttribute.Identifier == identifier)
                                {
                                    List<object> parameters = new List<object>();

                                    foreach (var parameter in method.GetParameters() )
                                    {
                                        IIncomingPacket incomingPacket = (IIncomingPacket)Activator.CreateInstance(parameter.ParameterType);

                                        incomingPacket.Read(reader);

                                        parameters.Add(incomingPacket);
                                    }

                                    Controller controller = (Controller)Activator.CreateInstance(type, server);

                                    controller.Context = new Context()
                                    {
                                        Request = new Request()
                                        {
                                            Connection = this
                                        },

                                        Response = new Response()
                                        {

                                        }
                                    };

                                    server.Dispatcher.QueueForExecution( () =>
                                    {
                                        IActionResult actionResult = (IActionResult)method.Invoke(controller, parameters.ToArray() );
                                    
                                        actionResult.Execute(controller.Context);
                                    } );
                                }
                            }
                        }                                        
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
}