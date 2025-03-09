using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class EditListDialogIncomingPacket : IIncomingPacket
    {
        public byte DoorId { get; set; }

        public uint WindowId { get; set; }

        public string Text { get; set; }

        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            DoorId = reader.ReadByte();

            WindowId = reader.ReadUInt();

            Text = reader.ReadString();
        }
    }
}