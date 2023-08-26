using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class StorageCollection : IStorageCollection
    {
        private Dictionary<int, int> storages = new Dictionary<int, int>();

        public bool TryGetValue(int key, out int value)
        {
            return storages.TryGetValue(key, out value);
        }

        public void SetValue(int key, int value)
        {
            storages[key] = value;
        }

        public void RemoveValue(int key)
        {
            storages.Remove(key);
        }
                
        public IEnumerable< KeyValuePair<int, int> > GetIndexed()
        {
            foreach (var item in storages)
            {
                yield return item;
            }
        }
    }
}