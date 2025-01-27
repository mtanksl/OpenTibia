using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common
{
    public static class Formula
    {
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

        public static ulong FixRequiredExperience(ushort level, ulong experience)
        {
            ulong minExperience = GetRequiredExperience(level);

            ulong maxExperience = GetRequiredExperience( (ushort)(level + 1) );

            if (experience >= minExperience && experience < maxExperience)
            {
                return experience;
            }

            return minExperience;
        }

        public static byte GetLevelPercent(ushort level, ulong experience)
        {
            ulong minExperience = GetRequiredExperience(level);

            ulong maxExperience = GetRequiredExperience( (ushort)(level + 1) );

            return (byte)Math.Max(0, Math.Min(100, Math.Floor(100.0 * (experience - minExperience) / (maxExperience - minExperience) ) ) );
        }

        public static double GetLossPercent(ushort level, byte levelPercent, ulong experience, Vocation vocation, int blesses)
        {
            static double MaximumExperienceLoss(ushort level, byte levelPercent, ulong experience)
            {
                if (level < 24)
                {
                    // 10 % for level 23 and below

                    return 10 / 100.0; 
                }
                else
                {
                    // 50 * (x + 50) * (x² - 5 * x + 8) / experience % for level 24 and above

                    double x = level + (levelPercent / 100.0);

                    return ( (50 * x * x * x) + (2250 * x * x) - (12100 * x) + 20000 ) / experience / 100.0; 
                }
            }

            double lossPercent = MaximumExperienceLoss(level, levelPercent, experience);

            VocationConfig vocationConfig = Context.Current.Server.Vocations.GetVocationById( (byte)vocation);
            
            if (vocationConfig.Promoted)
            {
                // reduce 30 % if player is promoted

                lossPercent *= 70 / 100.0; 
            }

            // reduce 8 % per bless

            lossPercent *= Math.Pow(92 / 100.0, blesses); 

            return lossPercent;
        }

        private static double[] containerLosses = new double[] { 100 / 100.0, 70 / 100.0, 45 / 100.0, 25 / 100.0, 10 / 100.0, 0 };

        public static double GetContainerLossPercent(int blesses)
        {
            double containerLossPercent = containerLosses[Math.Min(blesses, containerLosses.Length - 1) ];

            return containerLossPercent;
        }

        private static double[] equipmentLosses = new double[] { 10 / 100.0, 7 / 100.0, 4.5 / 100.0, 2.5 / 100.0, 1 / 100.0, 0 };
       
        public static double GetEquipmentLossPercent(int blesses)
        {
            double equipmentLossPercent = equipmentLosses[Math.Min(blesses, equipmentLosses.Length - 1) ];

            return equipmentLossPercent;
        }

        private static Dictionary<Skill, ushort> skillConstants = new Dictionary<Skill, ushort>()
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

        private static Dictionary<Skill, Dictionary<Vocation, Dictionary<byte, ulong>>> skillPointsCaches = new Dictionary<Skill, Dictionary<Vocation, Dictionary<byte, ulong>>>();

        public static ulong GetRequiredSkillPoints(Skill skill, Vocation vocation, byte skillLevel)
        {
            static ulong GetRequiredSkillPoints(Skill skill, Vocation vocation, byte skillLevel, ushort skillConstant, double vocationConstant)
            {
                Dictionary<Vocation, Dictionary<byte, ulong>> vocationsCache;

                if ( !skillPointsCaches.TryGetValue(skill, out vocationsCache) )
                {
                    vocationsCache = new Dictionary<Vocation, Dictionary<byte, ulong>>();

                    skillPointsCaches.Add(skill, vocationsCache);
                }

                Dictionary<byte, ulong> skillPointCache;

                if ( !vocationsCache.TryGetValue(vocation, out skillPointCache) )
                {
                    skillPointCache = new Dictionary<byte, ulong>();

                    vocationsCache.Add(vocation, skillPointCache);
                }

                ulong skillPoints;

                if ( !skillPointCache.TryGetValue(skillLevel, out skillPoints) )
                {
                    if (skill == Skill.MagicLevel)
                    {
                        skillPoints = (ulong)Math.Min(ulong.MaxValue, skillConstant * (Math.Pow(vocationConstant, skillLevel) - 1) / (vocationConstant - 1) );
                    }
                    else
                    {
                        skillPoints = (ulong)Math.Min(ulong.MaxValue, skillConstant * (Math.Pow(vocationConstant, skillLevel - 10) - 1) / (vocationConstant - 1) );
                    }

                    skillPointCache.Add(skillLevel, skillPoints);
                }

                return skillPoints;
            }

            VocationConfig vocationConfig = Context.Current.Server.Vocations.GetVocationById( (byte)vocation);

            return GetRequiredSkillPoints(skill, vocation, skillLevel, skillConstants[skill], vocationConfig.VocationConstants.GetValue(skill) );
        }

        public static byte FixRequiredSkillLevel(Player player, Skill skill)
        {
            byte skillLevel = player.Skills.GetSkillLevel(skill);

            if (skill != Skill.MagicLevel)
            {
                if (skillLevel < 10)
                {
                    return 10;
                }
            }

            return skillLevel;
        }

        public static ulong FixRequiredSkillPoints(Player player, Skill skill)
        {
            byte skillLevel = player.Skills.GetSkillLevel(skill);

            ulong skillPoints = player.Skills.GetSkillPoints(skill);

            ulong minSkillPoints = GetRequiredSkillPoints(skill, player.Vocation, skillLevel);

            ulong maxSkillPoints = GetRequiredSkillPoints(skill, player.Vocation, (byte)(skillLevel + 1) );

            if (skillPoints >= minSkillPoints && skillPoints < maxSkillPoints)
            {
                return skillPoints;
            }

            return minSkillPoints;
        }

        public static byte GetSkillPercent(Player player, Skill skill)
        {
            byte skillLevel = player.Skills.GetSkillLevel(skill);

            ulong skillPoints = player.Skills.GetSkillPoints(skill);

            ulong minSkillPoints = GetRequiredSkillPoints(skill, player.Vocation, skillLevel);

            ulong maxSkillPoints = GetRequiredSkillPoints(skill, player.Vocation, (byte)(skillLevel + 1) );

            return (byte)Math.Max(0, Math.Min(100, Math.Floor(100.0 * (skillPoints - minSkillPoints) / (maxSkillPoints - minSkillPoints) ) ) );
        }

        public static ushort GetBaseSpeed(ushort level)
        {
            return (ushort)(2 * level + 218);
        }

        public static ushort HasteFormula(ushort baseSpeed)
        {
            return (ushort)(1.3 * (baseSpeed - 40) + 40);
        }

        public static ushort StrongHasteFormula(ushort baseSpeed)
        {
            return (ushort)(1.7 * (baseSpeed - 40) + 40);
        }

        public static ushort SwiftFootFormula(int level, ushort baseSpeed)
        {
            return (ushort)(1.8 * (baseSpeed - 40) + 40);
        }

        public static ushort ChargeFormula(int level, ushort baseSpeed)
        {                        
            return (ushort)(1.9 * (baseSpeed - 40) + 40);
        }

        public static ushort AdrenalineBurst(int level, ushort baseSpeed)
        {                        
            return (ushort)(2.5 * (baseSpeed - 40) + 40);
        }

        public static (int Min, int Max) GenericFormula(int level, int magicLevel, double minx, double miny, double maxx, double maxy)
        {
            return ( (int)(level * 0.2 + magicLevel * minx + miny), (int)(level * 0.2 + magicLevel * maxx + maxy) );
        }

        public static (int Min, int Max) WhirlwindThrowFormula(ushort level, byte skill, int weapon)
        {
             return ( (int)( (skill + weapon) * 0.3 + level * 0.2), (int)(skill + weapon + level * 0.2) );
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

        public static (int Min, int Max) EtherealSpearFormula(ushort level, byte skill)
        {
            return ( (int)( (skill + 25) * 0.3 + level * 0.2), (int)(skill + 25 + level * 0.2) );
        }

        public static int DefenseFormula(int defense, FightMode fightMode)
        {
            int minDefense = (int)(defense / 2.0);

            int maxDefense = defense;

            double defenseFactor = (fightMode == FightMode.Offensive ? 0.5 : fightMode == FightMode.Balanced ? 0.75 : 1);

            return (int)(Context.Current.Server.Randomization.Take(minDefense, maxDefense) * defenseFactor);
        }

        public static int ArmorFormula(int armor)
        {
            int minArmor;

            int maxArmor;

            if (armor < 2)
            {
                minArmor = 0;

                maxArmor = 0;
            }
            else
            {
                minArmor = (int)(armor / 2.0);

                maxArmor = (int)(armor / 2.0) * 2 - 1;
            }
            
            return Context.Current.Server.Randomization.Take(minArmor, maxArmor);
        }

        public static (int Min, int Max) MeleeFormula(int level, int skill, int attack, FightMode fightMode)
        {
            int min = 0;

            int max = (int)(0.085 * (fightMode == FightMode.Offensive ? 1 : fightMode == FightMode.Balanced ? 0.75 : 0.5) * skill * attack) + (int)(level * 0.2);

            return (min, max);
        }

        public static (int Min, int Max) DistanceFormula(int level, int skill, int attack, FightMode fightMode)
        {
            int min = (int)(level * 0.2);

            int max = (int)(0.09 * (fightMode == FightMode.Offensive ? 1 : fightMode == FightMode.Balanced ? 0.75 : 0.5) * skill * attack) + min;

            return (min, max);
        }

        public static (int Min, int Max) DistanceFormula(int level, int skill, int attack, FightMode fightMode, int? weaponHitChance, int? weaponMaxHitChance, int distance)
        {
            double hitChancePercent;

            if (weaponHitChance == null)
            {
                if (weaponMaxHitChance == 75) // One handed
                {
                    if (distance == 1)
                    {
                        hitChancePercent = Math.Min(75, skill + 1) / 100.0;
                    }
                    else if (distance == 2)
                    {
                        hitChancePercent = Math.Min(75, (int)(2.4 * skill + 8) ) / 100.0;
                    }
                    else if (distance == 3)
                    {
                        hitChancePercent = Math.Min(75, (int)(1.55 * skill + 6) ) / 100.0;
                    }
                    else if (distance == 4)
                    {
                        hitChancePercent = Math.Min(75, (int)(1.25 * skill + 3) ) / 100.0;
                    }
                    else if (distance == 5)
                    {
                        hitChancePercent = Math.Min(75, skill + 1) / 100.0;
                    }
                    else if (distance == 6)
                    {
                        hitChancePercent = Math.Min(75, 0.8 * skill + 3) / 100.0;
                    }
                    else
                    {
                        hitChancePercent = Math.Min(75, 0.7 * skill + 2) / 100.0;
                    }
                }
                else if (weaponMaxHitChance == 90) // Two handed
                {
                    if (distance == 1)
                    {
                        hitChancePercent = Math.Min(90, 1.2 * skill + 1) / 100.0;
                    }
                    else if (distance == 2)
                    {
                        hitChancePercent = Math.Min(90, 3.2 * skill) / 100.0;
                    }
                    else if (distance == 3)
                    {
                        hitChancePercent = Math.Min(90, 2 * skill) / 100.0;
                    }
                    else if (distance == 4)
                    {
                        hitChancePercent = Math.Min(90, 1.55 * skill) / 100.0;
                    }
                    else if (distance == 5)
                    {
                        hitChancePercent = Math.Min(90, 1.2 * skill + 1) / 100.0;
                    }
                    else
                    {
                        hitChancePercent = Math.Min(90, skill) / 100.0;
                    }
                }
                else if (weaponMaxHitChance == 100)
                {
                    if (distance == 1)
                    {
                        hitChancePercent = Math.Min(100, 1.35 * skill + 1) / 100.0;
                    }
                    else if (distance == 2)
                    {
                        hitChancePercent = Math.Min(100, 3.2 * skill + 5) / 100.0;
                    }
                    else if (distance == 3)
                    {
                        hitChancePercent = Math.Min(100, 2.25 * skill + 2) / 100.0;
                    }
                    else if (distance == 4)
                    {
                        hitChancePercent = Math.Min(100, 1.5 * skill + 2) / 100.0;
                    }
                    else if (distance == 5)
                    {
                        hitChancePercent = Math.Min(100, 1.35 * skill + 1) / 100.0;
                    }
                    else
                    {
                        hitChancePercent = Math.Min(100, 1.2 * skill - 4) / 100.0;
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                hitChancePercent = weaponHitChance.Value / 100.0;
            }

            if (Context.Current.Server.Randomization.HasProbability(hitChancePercent) )
            {
                return DistanceFormula(level, skill, attack, fightMode);
            }

            return (0, 0);
        }

        public static (int Min, int Max) WandFormula(int attackStrength, int attackVariation)
        {
            int min = attackStrength - attackVariation;

            int max = attackStrength + attackVariation;

            return (min, max);
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

        public static Item GetShield(Player player)
        {
            Item item = (Item)player.Inventory.GetContent( (byte)Slot.Left);

            if (item != null && (item.Metadata.WeaponType == WeaponType.Shield) )
            {
                return item;
            }

            item = (Item)player.Inventory.GetContent( (byte)Slot.Right);

            if (item != null && (item.Metadata.WeaponType == WeaponType.Shield) )
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
    }
}