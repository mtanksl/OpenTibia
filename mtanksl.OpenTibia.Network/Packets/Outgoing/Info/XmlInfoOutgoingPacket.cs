using OpenTibia.IO;
using OpenTibia.Network.Packets.Outgoing;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace OpenTibia.Network.Packets.Incoming
{
    public class XmlInfoOutgoingPacket : IOutgoingPacket
    {
        public XmlInfoOutgoingPacket(XElement xml)
        {
            this.Xml = xml;
        }

        public XElement Xml { get; set; }

        public void Write(IByteArrayStreamWriter writer)
        {
            using (var memoryStream = new MemoryStream() )
            {
                using (var xmlWriter = XmlWriter.Create(memoryStream, new XmlWriterSettings() { Encoding = writer.Encoding } ) )
                {
                    Xml.WriteTo(xmlWriter);
                }

                writer.Write(memoryStream.ToArray() );
            }
        }
    }
}