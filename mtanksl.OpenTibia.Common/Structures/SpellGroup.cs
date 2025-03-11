namespace OpenTibia.Common.Structures
{
    public enum SpellGroup : byte
    {
        None = 0,

        Attack = 1,

        Healing = 2,

        Support = 3,

        Special = 4
    }

    public static class SpellGroupExtensions
    {
        public static SpellGroup FromString(string group)
        {
            switch (group)
            {
                case "Attack":

                    return SpellGroup.Attack;

                case "Healing":

                    return SpellGroup.Healing;

                case "Support":

                    return SpellGroup.Support;

                case "Special":

                    return SpellGroup.Special;
            }

            return SpellGroup.None;
        }
    }
}