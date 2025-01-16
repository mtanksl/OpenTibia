using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class SkillsMultiplier
    {
        public double MagicLevel { get; set; }

        public double Fist { get; set; }

        public double Club { get; set; }

        public double Sword { get; set; }

        public double Axe { get; set; }

        public double Distance { get; set; }

        public double Shield { get; set; }

        public double Fish { get; set; }

        public double GetSkillMultiplier(Skill skill)
        {
            switch (skill)
            {
                case Skill.MagicLevel:

                    return MagicLevel;

                case Skill.Fist:

                    return Fist;

                case Skill.Club:

                    return Club;

                case Skill.Sword:

                    return Sword;

                case Skill.Axe:

                    return Axe;

                case Skill.Distance:

                    return Distance;

                case Skill.Shield:

                    return Shield;

                case Skill.Fish:

                    return Fish;
            }

            throw new NotImplementedException();
        }
    }
}