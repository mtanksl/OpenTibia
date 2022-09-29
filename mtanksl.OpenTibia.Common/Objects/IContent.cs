using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public interface IContent
    {
        TopOrder TopOrder { get; }

        IContainer Parent { get; set; }
    }
}