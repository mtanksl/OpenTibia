using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.IO;
using OpenTibia.Network.Sockets;
using OpenTibia.Security;
using System.Net.Sockets;

namespace OpenTibia.Common
{
    public class LoginConnection : TibiaConnection
    {
        public LoginConnection(IServer server, Socket socket) : base(server, socket)
        {

        }

        protected override void OnConnected()
        {
            server.Logger.WriteLine("Connected on login server", LogLevel.Debug);

            base.OnConnected();
        }

        private bool first = true;

        protected override void OnReceived(byte[] body, int length)
        {
            ByteArrayArrayStream stream = new ByteArrayArrayStream(body);

            ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);

            try
            {
                if ( !server.Features.HasFeatureFlag(FeatureFlag.ProtocolChecksum) || Adler32.Generate(body, 4, length - 4) == reader.ReadUInt() )
                {
                    if (Keys == null)
                    {
                        int skip = 17;

                        if (server.Features.HasFeatureFlag(FeatureFlag.ProtocolChecksum) )
                        {
                            skip += 4;
                        }

                        if (server.Features.HasFeatureFlag(FeatureFlag.ClientVersion) )
                        {
                            skip += 4;
                        }

                        if (server.Features.HasFeatureFlag(FeatureFlag.PreviewState) )
                        {
                            skip += 1;
                        }

                        Rsa.DecryptAndReplace(body, skip, 128); // Keys, Account and Password

                        if (server.Features.HasFeatureFlag(FeatureFlag.Authenticator) )
                        {
                            Rsa.DecryptAndReplace(body, length - 128, 128); // AuthenticatorCode
                        }
                    }
                    else
                    {
                        //
                    }

                    byte identification = reader.ReadByte();

                    if (first)
                    {
                        first = false;

                        if (server.Features.LoginFirstCommands.TryGetValue(identification, out IPacketToCommand packetToCommand) )
                        {
                            Command command = packetToCommand.Convert(this, reader, server.Features);

                            server.Logger.WriteLine("Received on login server: 0x" + identification.ToString("X2") + " (" + packetToCommand.Name + ")", LogLevel.Debug);        

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
                        
                            Disconnect();
                        }
                    }
                    else
                    {
                        IncreaseInvalidMessage();

                        server.Logger.WriteLine("Invalid message received on login server", LogLevel.Warning);

                        server.Logger.WriteLine(body.Print(0, length), LogLevel.Warning);

                        Disconnect();
                    }
                }
                else
                {
                    IncreaseInvalidMessage();

                    server.Logger.WriteLine("Invalid message received on login server", LogLevel.Warning);

                    server.Logger.WriteLine(body.Print(0, length), LogLevel.Warning);
                    
                    Disconnect();
                }
            }
            catch
            {
                IncreaseInvalidMessage();

                server.Logger.WriteLine("Invalid message received on login server", LogLevel.Warning);

                server.Logger.WriteLine(body.Print(0, length), LogLevel.Warning);
                 
                Disconnect();           
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