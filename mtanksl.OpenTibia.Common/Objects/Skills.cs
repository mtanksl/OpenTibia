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

        public ulong MagicLevelPoints { get; set; }

        public byte MagicLevelPercent { get; set; }

        public byte Fist { get; set; }

        public ulong FistPoints { get; set; }

        public byte FistPercent { get; set; }

        public byte Club { get; set; }

        public ulong ClubPoints { get; set; }

        public byte ClubPercent { get; set; }

        public byte Sword { get; set; }

        public ulong SwordPoints { get; set; }

        public byte SwordPercent { get; set; }

        public byte Axe { get; set; }

        public ulong AxePoints { get; set; }

        public byte AxePercent { get; set; }

        public byte Distance { get; set; }

        public ulong DistancePoints { get; set; }

        public byte DistancePercent { get; set; }

        public byte Shield { get; set; }

        public ulong ShieldPoints { get; set; }

        public byte ShieldPercent { get; set; }

        public byte Fish { get; set; }

        public ulong FishPoints { get; set; }

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

        public ulong GetSkillPoints(Skill skill)
        {
            switch (skill)
            {
                case Skill.MagicLevel:
                    
                    return MagicLevelPoints;

                case Skill.Fist:

                    return FistPoints;

                case Skill.Club:

                    return ClubPoints;

                case Skill.Sword:

                    return SwordPoints;

                case Skill.Axe:

                    return AxePoints;

                case Skill.Distance:

                    return DistancePoints;

                case Skill.Shield:

                    return ShieldPoints;

                case Skill.Fish:

                    return FishPoints;
            }

            throw new NotImplementedException();
        }
    }
}