using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IStorageCollection
    {
        bool TryGetValue(int key, out int _value);

        void SetValue(int key, int value);

        void RemoveValue(int key);

        IEnumerable< KeyValuePair<int, int> > GetIndexed();
    }
}