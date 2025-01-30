using System.Text;

namespace OpenTibia.IO
{
    public interface IByteArrayStreamReader
    {
        IByteArrayStream BaseStream { get; }

        Encoding Encoding { get; }

        byte ReadByte();
        
        bool ReadBool();
        
        short ReadShort();
        
        ushort ReadUShort();
        
        int ReadInt();
        
        uint ReadUInt();
        
        long ReadLong();
        
        ulong ReadULong();
        
        string ReadString();
        
        string ReadString(int length);
        
        byte[] ReadBytes(int length);

        void ReadBytes(byte[] buffer, int offset, int count);
    }
}