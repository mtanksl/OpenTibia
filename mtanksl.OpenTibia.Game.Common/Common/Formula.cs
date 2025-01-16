using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common
{
    public static class Formula
    {
        public static (int Min, int Max) MeleeFormula(int level, int skill, int attack, FightMode fightMode)
        {
            int min = 0;

            int max = (int)Math.Floor(0.085 * (fightMode == FightMode.Offensive ? 1 : fightMode == FightMode.Balanced ? 0.75 : 0.5) * skill * attack) + (int)Math.Floor(level / 5.0);

            return (min, max);
        }

        public static (int Min, int Max) DistanceFormula(int level, int skill, int attack, FightMode fightMode)
        {
            int min = (int)Math.Floor(level / 5.0);

            int max = (int)Math.Floor(0.09 * (fightMode == FightMode.Offensive ? 1 : fightMode == FightMode.Balanced ? 0.75 : 0.5) * skill * attack) + min;

            return (min, max);
        }

        public static (int Min, int Max) WandFormula(int attackStrength, int attackVariation)
        {
            int min = attackStrength - attackVariation;

            int max = attackStrength + attackVariation;

            return (min, max);
        }

        public static (int Min, int Max) GenericFormula(int level, int magicLevel, double minx, double miny, double maxx, double maxy)
        {
            return ( (int)(level * 0.2 + magicLevel * minx + miny), (int)(level * 0.2 + magicLevel * maxx + maxy) );
        }

        public static (int Min, int Max) GenericFormula(int level, int magicLevel, int @base, int variation)
        {
            var formula = 3 * magicLevel + 2 * level;

            return (formula * (@base - variation) / 100, formula * (@base + variation) / 100);
        }

        public static ushort HasteFormula(ushort baseSpeed)
        {
            return (ushort)(baseSpeed * 1.3 - 24);
        }

        public static ushort StrongHasteFormula(ushort baseSpeed)
        {
            return (ushort)(baseSpeed * 1.7 - 56);
        }

        public static ushort SwiftFootFormula(int level, ushort baseSpeed)
        {
            return (ushort)(baseSpeed + Math.Floor( (level * 1.6 + 110) / 2) * 2);
        }

        public static ushort ChargeFormula(int level, ushort baseSpeed)
        {                        
            return (ushort)(baseSpeed + Math.Floor( (level * 1.8 + 123.3) / 2) * 2);
        }

        public static (int Min, int Max) GroundshakerFormula(int level, int skill, int weapon)
        {
            return ( (int)( (skill + weapon) * 0.5 + level * 0.2), (int)( (skill + weapon) * 1.1 + level * 0.2) );
        }

        public static (int Min, int Max) BerserkFormula(int level, int skill, int weapon)
        {
            return ( (int)( (skill + weapon) * 0.5 + level * 0.2), (int)( (skill + weapon) * 1.5 + level * 0.2) );
        }

        public static (int Min, int Max) FierceBerserkFormula(int level, int skill, int weapon)
        {
            return ( (int)( (skill + weapon * 2) * 1.1 + level * 0.2), (int)( (skill + weapon * 2) * 3 + level * 0.2) );
        }

        public static (int Min, int Max) WhirlwindThrowFormula(ushort level, byte skill, int weapon)
        {
             return ( (int)( (skill + weapon) * 0.3 + level * 0.2), (int)(skill + weapon + level * 0.2) );
        }
          
        public static (int Min, int Max) EtherealSpearFormula(ushort level, byte skill)
        {
             return ( (int)( (skill + 25) * 0.3 + level * 0.2), (int)(skill + 25 + level * 0.2) );
        }

        public static Item GetKnightWeapon(Player player)
        {
            Item item = (Item)player.Inventory.GetContent( (byte)Slot.Left);

            if (item != null && (item.Metadata.WeaponType == WeaponType.Sword || item.Metadata.WeaponType == WeaponType.Club || item.Metadata.WeaponType == WeaponType.Axe) )
            {
                return item;
            }

            item = (Item)player.Inventory.GetContent( (byte)Slot.Right);

            if (item != null && (item.Metadata.WeaponType == WeaponType.Sword || item.Metadata.WeaponType == WeaponType.Club || item.Metadata.WeaponType == WeaponType.Axe) )
            {
                return item;
            }

            return null;
        }

        public static Item GetWeapon(Player player)
        {
            Item item = (Item)player.Inventory.GetContent( (byte)Slot.Left);

            if (item != null && (item.Metadata.WeaponType == WeaponType.Sword || item.Metadata.WeaponType == WeaponType.Club || item.Metadata.WeaponType == WeaponType.Axe || item.Metadata.WeaponType == WeaponType.Distance || item.Metadata.WeaponType == WeaponType.Wand) )
            {
                return item;
            }

            item = (Item)player.Inventory.GetContent( (byte)Slot.Right);

            if (item != null && (item.Metadata.WeaponType == WeaponType.Sword || item.Metadata.WeaponType == WeaponType.Club || item.Metadata.WeaponType == WeaponType.Axe || item.Metadata.WeaponType == WeaponType.Distance || item.Metadata.WeaponType == WeaponType.Wand) )
            {
                return item;
            }

            return null;
        }

        public static Item GetAmmunition(Player player)
        {
            Item item = (Item)player.Inventory.GetContent( (byte)Slot.Extra);

            if (item != null && item.Metadata.WeaponType == WeaponType.Ammo)
            {
                return item;
            }

            return null;
        }

        private static Dictionary<ushort, ulong> experienceCache = new Dictionary<ushort, ulong>();

        public static ulong GetRequiredExperience(ushort level)
        {
            ulong experience;

            if ( !experienceCache.TryGetValue(level, out experience) )
            {
                experience = (ulong)( ( 50 * Math.Pow(level - 1, 3) - 150 * Math.Pow(level - 1, 2) + 400 * (level - 1) ) / 3 );

                experienceCache.Add(level, experience);
            }

            return experience;
        }

        public static byte GetLevelPercent(ushort level, ulong experience)
        {
            ulong minExperience = GetRequiredExperience(level);

            ulong maxExperience = GetRequiredExperience( (ushort)(level + 1) );

            return (byte)Math.Ceiling(100.0 * (experience - minExperience) / (maxExperience - minExperience) );
        }

        private static Dictionary<Skill, int> skillConstants = new Dictionary<Skill, int>()
        {
            { Skill.MagicLevel, 1600 },

            { Skill.Fist, 50 },

            { Skill.Club, 50 },

            { Skill.Sword, 50 },

            { Skill.Axe, 50 },

            { Skill.Distance, 30 },

            { Skill.Shield, 100 },

            { Skill.Fish, 20 }
        };

        private static Dictionary<Skill, Dictionary<byte, ulong> > skillCaches = new Dictionary<Skill, Dictionary<byte, ulong> >()
        {
            { Skill.MagicLevel, new Dictionary<byte, ulong>() },

            { Skill.Fist, new Dictionary<byte, ulong>() },

            { Skill.Club, new Dictionary<byte, ulong>() },

            { Skill.Sword, new Dictionary<byte, ulong>() },

            { Skill.Axe, new Dictionary<byte, ulong>() },

            { Skill.Distance, new Dictionary<byte, ulong>() },

            { Skill.Shield, new Dictionary<byte, ulong>() },

            { Skill.Fish, new Dictionary<byte, ulong>() }
        };

        public static ulong GetRequiredSkillTries(byte skillLevel, Skill skill, VocationConfig vocationConfig)
        {
            Dictionary<byte, ulong> skillCache;

            skillCaches.TryGetValue(skill, out skillCache);

            ulong skillTries;

            if ( !skillCache.TryGetValue(skillLevel, out skillTries) )
            {
                if (skill == Skill.MagicLevel)
                {
                    skillTries = (ulong)(skillConstants[skill] * (Math.Pow(vocationConfig.VocationConstants.GetValue(skill), skillLevel) - 1 ) / (vocationConfig.VocationConstants.GetValue(skill) - 1) );
                }
                else
                {
                    skillTries = (ulong)(skillConstants[skill] * (Math.Pow(vocationConfig.VocationConstants.GetValue(skill), skillLevel - 10) - 1 ) / (vocationConfig.VocationConstants.GetValue(skill) - 1) );
                }

                skillCache[skillLevel] = skillTries;
            }

            return skillTries;
        }

        public static byte GetSkillPercent(byte skillLevel, ulong skillTries, Skill skill, VocationConfig vocationConfig)
        {
            ulong minSkillTries = GetRequiredSkillTries(skillLevel, skill, vocationConfig);

            ulong maxSkillTries = GetRequiredSkillTries( (byte)(skillLevel + 1), skill, vocationConfig);

            return (byte)Math.Ceiling(100.0 * (skillTries - minSkillTries) / (maxSkillTries - minSkillTries) );
        }
    }
}