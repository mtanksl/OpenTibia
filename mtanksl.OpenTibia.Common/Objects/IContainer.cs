using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IContainer
    {
        byte AddContent(IContent content);

        void AddContent(byte index, IContent content);

        byte RemoveContent(IContent content);
        
        byte ReplaceContent(IContent before, IContent after);

        byte GetIndex(IContent content);

        IContent GetContent(byte index);

        IEnumerable<IContent> GetContents();

        IEnumerable< KeyValuePair<byte, IContent> > GetIndexedContents();
    }
}