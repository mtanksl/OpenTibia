using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IContainer
    {
        byte AddContent(IContent content);

        void AddContent(IContent content, byte index);

        void ReplaceContent(byte index, IContent content);

        void RemoveContent(byte index);
        
        byte GetIndex(IContent content);

        bool TryGetIndex(IContent content, out byte index);

        IContent GetContent(byte index);

        IEnumerable<IContent> GetContents();

        IEnumerable< KeyValuePair<byte, IContent> > GetIndexedContents();
    }
}