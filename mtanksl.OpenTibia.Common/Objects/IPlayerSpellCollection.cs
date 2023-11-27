using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IPlayerSpellCollection
    {
        void SetSpell(string name);

        void RemoveSpell(string name);

        IEnumerable<string> GetSpells();
    }
}