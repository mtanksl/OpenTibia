using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class PlayerBlessCollection
    {
        private HashSet<string> blesses = new HashSet<string>();

        public int Count
        {
            get
            {
                return blesses.Count;
            }
        }

        public bool HasBless(string name)
        {
            return blesses.Contains(name);
        }

        public void SetBless(string name)
        {
            blesses.Add(name);
        }

        public void RemoveBless(string name)
        {
            blesses.Remove(name);
        }

        public void ClearBlesses()
        {
            blesses.Clear();
        }

        public IEnumerable<string> GetBlesses()
        {
            return blesses;
        }
    }
}