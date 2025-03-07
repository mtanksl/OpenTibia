using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.IO;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Network.Sockets;
using OpenTibia.Security;
using System.Collections.Generic;
using System.Net.Sockets;

namespace OpenTibia.Common
{
    public class GameConnection : TibiaConnection
    {        
		private IServer server;

        public GameConnection(IServer server, Socket socket) : base(server, socket)
        {
            this.server = server;
        }

        protected override void OnConnected()
        {
            server.Logger.WriteLine("Connected on game server", LogLevel.Debug);

            server.QueueForExecution( () =>
            {
                Context.Current.AddPacket(this, new SendConnectionInfoOutgoingPacket(0) );

                return Promise.Completed;
            } );

            base.OnConnected();
        }

        private bool first = true;

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
                        Rsa.DecryptAndReplace(body, 9, length - 9);
                    }
                    else
                    {
                        Xtea.DecryptAndReplace(body, 4, length - 4, 32, Keys);

                        stream.Seek(Origin.Current, 2);
                    }

                    byte identification = reader.ReadByte();

                    if (first)
                    {
                        first = false;

                        if (server.Features.GameFirstCommands.TryGetValue(identification, out IPacketToCommand packetToCommand) )
                        {
                            Command command = packetToCommand.Convert(this, reader);

                            server.Logger.WriteLine("Received on game server: 0x" + identification.ToString("X2") + " (" + packetToCommand.Name + ")", LogLevel.Debug);

                            server.QueueForExecution( () =>
                            {
                                return Context.Current.AddCommand(command);
                            } );
                        }
                        else
                        {                               
                            IncreaseUnknownPacket();

                            server.Logger.WriteLine("Unknown packet received on game server: 0x" + identification.ToString("X2"), LogLevel.Warning);

                            server.Logger.WriteLine(body.Print(0, length), LogLevel.Warning);
                        }
                    }
                    else
                    {
                        if (Client == null || Client.Player == null || Client.Player.Tile == null || Client.Player.IsDestroyed)
                        {
							if (identification != 0x87)
							{
								Disconnect();
							}
                        }
                        else
                        {
							Dictionary<byte, IPacketToCommand> current;

							if (Client.AccountManagerType == AccountManagerType.None)
							{
								current = server.Features.GameCommands;
                            }
							else
							{
								current = server.Features.GameAccountManagerCommands;
							}

                            if (current.TryGetValue(identification, out IPacketToCommand packetToCommand) )
                            {
                                Command command = packetToCommand.Convert(this, reader);

                                server.Logger.WriteLine("Received on game server: 0x" + identification.ToString("X2") + " (" + packetToCommand.Name + ")", LogLevel.Debug);

                                server.QueueForExecution( () =>
                                {
                                    return Context.Current.AddCommand(command);
                                } );
                            }
                            else
                            {                         
                                IncreaseUnknownPacket();

                                server.Logger.WriteLine("Unknown packet received on game server: 0x" + identification.ToString("X2"), LogLevel.Warning);

                                server.Logger.WriteLine(body.Print(0, length), LogLevel.Warning);
                            }
                        }
                    }
                }
                else
                {
                    IncreaseInvalidMessage();

                    server.Logger.WriteLine("Invalid message received on game server", LogLevel.Warning);

                    server.Logger.WriteLine(body.Print(0, length), LogLevel.Warning);
                }
            }
            catch
            {
                IncreaseInvalidMessage();

                server.Logger.WriteLine("Invalid message received on game server", LogLevel.Warning);

                server.Logger.WriteLine(body.Print(0, length), LogLevel.Warning);
            }

            base.OnReceived(body, length);
        }

        protected override void OnDisconnected(DisconnectedEventArgs e)
        {
            server.Logger.WriteLine("Disconnected on game server", LogLevel.Debug);

            if (Client == null || Client.Player == null || Client.Player.Tile == null || Client.Player.IsDestroyed)
            {
                    
            }
            else
            {
                server.QueueForExecution( () =>
                {
					return Context.Current.AddCommand(new ShowMagicEffectCommand(Client.Player, MagicEffectType.Puff) ).Then( () =>
					{
						return Context.Current.AddCommand(new CreatureDestroyCommand(Client.Player) );
					} );
                } );
            }
            
            base.OnDisconnected(e);
        }
    }
}