using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class StorageCollection : IStorageCollection
    {
        private Dictionary<int, int> storages = new Dictionary<int, int>();

        public bool ContainsKey(int key)
        {
            int _value;

            if (storages.TryGetValue(key, out _value) )
            {
                return true;
            }

            return false;
        }

        public bool ContainsValue(int key, int value)
        {
            int _value;

            if (storages.TryGetValue(key, out _value) )
            {
                if (value == _value)
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryGetValue(int key, out int _value)
        {
            return storages.TryGetValue(key, out _value);
        }

        public void SetValue(int key, int value)
        {
            storages[key] = value;
        }

        public void RemoveValue(int key)
        {
            storages.Remove(key);
        }
    }
}