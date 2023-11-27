using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class PlayerSpellCollection : IPlayerSpellCollection
    {
        private HashSet<string> spells = new HashSet<string>();

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