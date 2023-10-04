using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IContainer
    {
        int AddContent(IContent content);

        void AddContent(IContent content, int index);

        void ReplaceContent(int index, IContent content);

        void RemoveContent(int index);

        int GetIndex(IContent content);

        bool TryGetIndex(IContent content, out int index);

        IContent GetContent(int index);

        IEnumerable<IContent> GetContents();

        IEnumerable< KeyValuePair<int, IContent> > GetIndexedContents();
    }
}