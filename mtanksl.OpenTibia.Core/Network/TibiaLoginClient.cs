using OpenTibia.IO;
using OpenTibia.Security;
using System;
using System.Net.Sockets;

namespace OpenTibia
{
    public partial class TibiaLoginClient : TibiaClient
    {
        public TibiaLoginClient(Socket socket) : base(socket)
        {

        }
        
        protected override void OnReceive(byte[] bytes, bool first)
        {
            ByteArrayArrayStream stream = new ByteArrayArrayStream(bytes);

            ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);

            try
            {
                if (Adler32.Generate(bytes, 4) == reader.ReadUInt() )
                {
                    if (Keys == null)
                    {
                        Rsa.DecryptAndReplace(bytes, 21);
                    }

                    byte identifier = reader.ReadByte();

                        Game.Current.Log.WriteLine("Received 0x{0:X2}", identifier);

                    if (first)
                    {
                        switch (identifier)
                        {
                            case 0x01:

                                ExecuteInContext<EnterGameIncomingPacket>(reader, packet =>
                                {
                                    EnterGame(packet);
                                } );

                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Game.Current.Log.WriteLine("Exception {0}", e.Message);
            }
        }
    }
}