using System.Collections.Generic;

namespace OpenTibia.Game.Objects
{
    public interface IContainer
    {
        int AddContent(IContent content);

        void AddContent(int index, IContent content);

        int RemoveContent(IContent content);

        int ReplaceContent(IContent before, IContent after);

        int GetIndex(IContent content);

        IContent GetContent(int index);

        IEnumerable<IContent> GetContents();
    }
}