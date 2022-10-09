using OpenTibia.IO;
using OpenTibia.Security;
using System;
using System.Net.Sockets;

namespace OpenTibia.Proxy
{
    public class LoginProxyConnection : ProxyConnection
    {
        private Logger logger;

        public LoginProxyConnection(Logger logger, Socket socket, string host, int port) : base(socket, host, port)
        {
            this.logger = logger;
        }

        protected override void OnReceivedBodyFromClient(byte[] body)
        {
            base.OnReceivedBodyFromClient(body);

            try
            {
                ByteArrayArrayStream stream = new ByteArrayArrayStream(body);

                ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);
#if V_772
                if (Keys == null)
                {
                    Rsa.DecryptAndReplace(body, 17);
                }
                else
                {
                    Xtea.DecryptAndReplace(body, 0, 32, Keys);

                    stream.Seek(Origin.Current, 2);
                }

                byte identification = reader.ReadByte();

                if (identification == 0x01)
                {
                    stream.Seek(Origin.Current, 17);

                    Keys = new uint[]
                    {
                        reader.ReadUInt(),

                        reader.ReadUInt(),

                        reader.ReadUInt(),

                        reader.ReadUInt()
                    };
                }

                logger.WriteLine("Client: " + body.Print(), LogLevel.Debug);
#else
                if (Adler32.Generate(body, 4) == reader.ReadUInt() )
                {
                    if (Keys == null)
                    {
                        Rsa.DecryptAndReplace(body, 21);
                    }
                    else
                    {
                        Xtea.DecryptAndReplace(body, 4, 32, Keys);

                        stream.Seek(Origin.Current, 2);
                    }

                    byte identification = reader.ReadByte();

                    if (identification == 0x01)
                    {
                        stream.Seek(Origin.Current, 17);

                        Keys = new uint[]
                        {
                            reader.ReadUInt(),

                            reader.ReadUInt(),

                            reader.ReadUInt(),

                            reader.ReadUInt()
                        };
                    }

                    logger.WriteLine("Client: " + body.Print(), LogLevel.Debug);
                }
                else
                {
                    logger.WriteLine("Client: " + body.Print(), LogLevel.Warning);
                }
#endif
            }
            catch (Exception ex)
            {
                logger.WriteLine("Client: " + ex.ToString(), LogLevel.Error);
            }
        }

        protected override void OnReceivedBodyFromServer(byte[] body)
        {
            base.OnReceivedBodyFromServer(body);

            try
            {
                ByteArrayArrayStream stream = new ByteArrayArrayStream(body);

                ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);
#if V_772
                if (Keys == null)
                {
                    stream.Seek(Origin.Current, 2);
                }
                else
                {
                    Xtea.DecryptAndReplace(body, 0, 32, Keys);

                    stream.Seek(Origin.Current, 2);
                }

                byte identification = reader.ReadByte();

                logger.WriteLine("Server: " + body.Print(), LogLevel.Information);
#else
                if (Adler32.Generate(body, 4) == reader.ReadUInt() )
                {
                    if (Keys == null)
                    {
                        stream.Seek(Origin.Current, 2);
                    }
                    else
                    {
                        Xtea.DecryptAndReplace(body, 4, 32, Keys);

                        stream.Seek(Origin.Current, 2);
                    }

                    byte identification = reader.ReadByte();

                    logger.WriteLine("Server: " + body.Print(), LogLevel.Information);
                }
                else
                {
                    logger.WriteLine("Server: " + body.Print(), LogLevel.Warning);
                }
#endif
            }
            catch (Exception ex)
            {
                logger.WriteLine("Server: " + ex.ToString(), LogLevel.Error);
            }
        }
    }
}