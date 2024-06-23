using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class PlayerBlessCollection
    {
        private HashSet<string> blesses = new HashSet<string>();

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

        public IEnumerable<string> GetBlesses()
        {
            return blesses;
        }
    }
}