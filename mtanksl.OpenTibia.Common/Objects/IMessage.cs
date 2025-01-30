using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public interface IMessage
    {
        byte[] GetBytes(MessageProtocol type, uint[] keys);
    }
}