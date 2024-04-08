using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class PlayerSpellCollection
    {
        private HashSet<string> spells = new HashSet<string>();

        public bool HasSpell(string name)
        {
            return spells.Contains(name);
        }

        public void SetSpell(string name)
        {
            spells.Add(name);
        }

        public void RemoveSpell(string name)
        {
            spells.Remove(name);
        }

        public IEnumerable<string> GetSpells()
        {
            return spells;
        }
    }
}