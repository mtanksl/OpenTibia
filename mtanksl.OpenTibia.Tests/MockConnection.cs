using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;
using System.Threading;

namespace OpenTibia.Tests
{
    public class MockConnection : IConnection
    {
        public MockConnection(string ipAddress)
        {
            this.ipAddress = ipAddress;
        }

        private string ipAddress;

        public string IpAddress
        {
            get
            {
                return ipAddress;
            }
        }

        private IClient client;

        public IClient Client
        {
            get
            {
                return client;
            }
            set
            {
                if (value != client)
                {
                    var current = client;
                    
                                  client = value;

                    if (value == null)
                    {
                        current.Connection = null;
                    }
                    else
                    {
                        client.Connection = this;
                    }
                }
            }
        }

        public uint[] Keys { get; set; }

        private List<IOutgoingPacket> outgoingPackets;

        public void BeginRecord()
        {
            outgoingPackets = new List<IOutgoingPacket>();
        }

        public void Send(IMessageCollection messageCollection) 
        {
            if (messageCollection is MockMessageCollection mockMessageCollection)
            {
                outgoingPackets.AddRange(mockMessageCollection.OutgoingPackets);

                waitSend.Set();
            }
        }

        private bool isDisconnected;

        public bool IsDisconnected
        {
            get
            {
                return isDisconnected;
            }
        }

        public void Disconnect()
        {
            isDisconnected = true;

            waitDisconnect.Set();
        }

        private AutoResetEvent waitSend = new AutoResetEvent(false);

        private AutoResetEvent waitDisconnect = new AutoResetEvent(false);

        public IEnumerable<IOutgoingPacket> EndRecord()
        {
            // Because Promise.Wait() completes before Context.Flush()

            waitSend.WaitOne(10);

            waitDisconnect.WaitOne(10);

            return outgoingPackets;
        }
    }
}