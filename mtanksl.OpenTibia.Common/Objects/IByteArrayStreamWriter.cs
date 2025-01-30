using System.Text;

namespace OpenTibia.IO
{
    public interface IByteArrayStreamWriter
    {
        IByteArrayStream BaseStream { get; }

        Encoding Encoding { get; }

        void Write(byte value);

        void Write(bool value);

        void Write(short value);

        void Write(ushort value);

        void Write(int value);

        void Write(uint value);

        void Write(long value);

        void Write(ulong value);

        void Write(string value);

        void Write(byte[] buffer);

        void Write(byte[] buffer, int offset, int length);
    }
}