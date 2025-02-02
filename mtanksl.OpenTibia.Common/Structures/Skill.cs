namespace OpenTibia.Common.Structures
{
    public enum Skill : byte
    {
        MagicLevel = 0,

        Fist = 1,

        Club = 2,

        Sword = 3,

        Axe = 4,

        Distance = 5,

        Shield = 6,

        Fish = 7
    }

    public static class SkillExtensions
    {
        public static string GetDescription(this Skill skill)
        {
            switch (skill)
            {
                case Skill.MagicLevel:

                    return "magic level";

                case Skill.Fist:

                    return "fist fighting";

                case Skill.Club:

                    return "club fighting";

                case Skill.Sword:

                    return "sword fighting";

                case Skill.Axe:

                    return "axe fighting";

                case Skill.Distance:

                    return "distance fighting";

                case Skill.Shield:

                    return "shielding";

                case Skill.Fish:

                    return "fishing";
            }

            return null;
        }
    }
}