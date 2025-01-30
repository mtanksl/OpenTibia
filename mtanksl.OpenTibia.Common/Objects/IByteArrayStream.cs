namespace OpenTibia.IO
{
    public interface IByteArrayStream
    {
        int Position { get; }

        int Length { get; }

        void Seek(Origin origin, int offset);

        byte ReadByte();

        void Read(byte[] buffer, int offset, int count);

        void WriteByte(byte value);

        void Write(byte[] buffer, int offset, int count);
    }
}