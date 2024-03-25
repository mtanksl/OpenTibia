using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IPlayerSpellCollection
    {
        bool HasSpell(string name);

        void SetSpell(string name);

        void RemoveSpell(string name);

        IEnumerable<string> GetSpells();
    }
}