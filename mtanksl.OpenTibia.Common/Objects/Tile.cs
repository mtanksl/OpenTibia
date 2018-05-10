using System;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class Tile : IContainer
    {
        public int AddContent(IContent content)
        {
            throw new NotImplementedException();
        }

        public void AddContent(int index, IContent content)
        {
            throw new NotImplementedException();
        }

        public int RemoveContent(IContent content)
        {
            throw new NotImplementedException();
        }

        public int ReplaceContent(IContent before, IContent after)
        {
            throw new NotImplementedException();
        }

        public int GetIndex(IContent content)
        {
            throw new NotImplementedException();
        }

        public IContent GetContent(int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IContent> GetContents()
        {
            throw new NotImplementedException();
        }       
    }
}