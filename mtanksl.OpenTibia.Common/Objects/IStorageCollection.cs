namespace OpenTibia.Common.Objects
{
    public interface IStorageCollection
    {
        bool ContainsKey(int key);

        bool ContainsValue(int key, int value);

        bool TryGetValue(int key, out int _value);

        void SetValue(int key, int value);

        void RemoveValue(int key);
    }
}