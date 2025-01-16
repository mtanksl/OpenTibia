using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Common.Objects
{
    public class Skills
    {
        public Skills(Player player)
        {
            this.player = player;
        }

        private Player player;

        public Player Player
        {
            get
            {
                return player;
            }
        }

        public byte MagicLevel { get; set; }

        public ulong MagicLevelTries { get; set; }

        public byte MagicLevelPercent { get; set; }

        public byte Fist { get; set; }

        public ulong FistTries { get; set; }

        public byte FistPercent { get; set; }

        public byte Club { get; set; }

        public ulong ClubTries { get; set; }

        public byte ClubPercent { get; set; }

        public byte Sword { get; set; }

        public ulong SwordTries { get; set; }

        public byte SwordPercent { get; set; }

        public byte Axe { get; set; }

        public ulong AxeTries { get; set; }

        public byte AxePercent { get; set; }

        public byte Distance { get; set; }

        public ulong DistanceTries { get; set; }

        public byte DistancePercent { get; set; }

        public byte Shield { get; set; }

        public ulong ShieldTries { get; set; }

        public byte ShieldPercent { get; set; }

        public byte Fish { get; set; }

        public ulong FishTries { get; set; }

        public byte FishPercent { get; set; }

        public byte GetSkillLevel(Skill skill)
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

        public ulong GetSkillTries(Skill skill)
        {
            switch (skill)
            {
                case Skill.MagicLevel:
                    
                    return MagicLevelTries;

                case Skill.Fist:

                    return FistTries;

                case Skill.Club:

                    return ClubTries;

                case Skill.Sword:

                    return SwordTries;

                case Skill.Axe:

                    return AxeTries;

                case Skill.Distance:

                    return DistanceTries;

                case Skill.Shield:

                    return ShieldTries;

                case Skill.Fish:

                    return FishTries;
            }

            throw new NotImplementedException();
        }

        public byte GetSkillPercent(Skill skill)
        {
            switch (skill)
            {
                case Skill.MagicLevel:
                    
                    return MagicLevelPercent;

                case Skill.Fist:

                    return FistPercent;

                case Skill.Club:

                    return ClubPercent;

                case Skill.Sword:

                    return SwordPercent;

                case Skill.Axe:

                    return AxePercent;

                case Skill.Distance:

                    return DistancePercent;

                case Skill.Shield:

                    return ShieldPercent;

                case Skill.Fish:

                    return FishPercent;
            }

            throw new NotImplementedException();
        }
    }
}