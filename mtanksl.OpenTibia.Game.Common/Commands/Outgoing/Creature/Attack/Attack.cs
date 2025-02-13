using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public abstract class Attack
    {
        protected Attack(DamageType damageType, int min, int max, DamageType? attackModifierDamageType, int? attackModifierMin, int? attackModifierMax)
        {
            DamageType = damageType;

            Min = min;

            Max = max;

            AttackModifierDamageType = attackModifierDamageType;

            AttackModifierMin = attackModifierMin;

            AttackModifierMax = attackModifierMax;
        }

        public DamageType DamageType { get; }

        public int Min { get; }

        public int Max { get; }

        public DamageType? AttackModifierDamageType { get; }

        public int? AttackModifierMin { get; }

        public int? AttackModifierMax { get; }

        public virtual (int Damage, BlockType BlockType) Calculate(Creature attacker, Creature target)
        {
            if (target is Monster monster)
            {
                if (AttackModifierDamageType != null && AttackModifierMin != null && AttackModifierMax != null)
                {
                    BlockType blockType = BlockType.None;

                    int damage1 = Context.Current.Server.Randomization.Take(Min, Max);

                    int damage2 = Context.Current.Server.Randomization.Take(AttackModifierMin.Value, AttackModifierMax.Value);

                    if (monster.Metadata.Immunities.Contains(DamageType) )
                    {
                        damage1 = 0;
                    }
                
                    if (monster.Metadata.Immunities.Contains(AttackModifierDamageType.Value) )
                    {
                        damage2 = 0;
                    }

                    if (damage1 + damage2 <= 0)
                    {
                        blockType = BlockType.Immune;
                    }
                    else
                    {
                        if (monster.Metadata.Mitigation > 0)
                        {
                            double mitigationPercent = (100 - monster.Metadata.Mitigation) / 100.0;

                            damage1 = Math.Max(0, (int)(damage1 * mitigationPercent) );

                            damage2 = Math.Max(0, (int)(damage2 * mitigationPercent) );
                        }
                        else
                        {
                            int defense = Formula.DefenseFormula(monster.Metadata.Defense, FightMode.Balanced);

                            int diff = damage1 - defense;

                            if (diff >= 0)
                            {
                                damage1 = diff;

                                defense = 0;
                            }
                            else
                            {
                                damage1 = 0;

                                defense = -diff;
                            }

                            diff = damage2 - defense;

                            if (diff >= 0)
                            {
                                damage2 = diff;

                                defense = 0;
                            }
                            else
                            {
                                damage2 = 0;

                                defense = -diff;
                            }
                        }

                        if (damage1 + damage2 <= 0)
                        {
                            blockType = BlockType.Shield;
                        }
                        else
                        {
                            double elementPercent;

                            if ( !monster.Metadata.DamageTakenFromElements.TryGetValue(DamageType, out elementPercent) )
                            {
                                elementPercent = 1;
                            }

                            damage1 = Math.Max(0, (int)(damage1 * elementPercent) );

                            if ( !monster.Metadata.DamageTakenFromElements.TryGetValue(AttackModifierDamageType.Value, out elementPercent) )
                            {
                                elementPercent = 1;
                            }

                            damage2 = Math.Max(0, (int)(damage2 * elementPercent) );

                            if (damage1 + damage2 <= 0)
                            {
                                blockType = BlockType.Armor;
                            }
                            else
                            {
                                int armor = Formula.ArmorFormula(monster.Metadata.Armor);

                                int diff = damage1 - armor;

                                if (diff >= 0)
                                {
                                    damage1 = diff;

                                    armor = 0;
                                }
                                else
                                {
                                    damage1 = 0;

                                    armor = -diff;
                                }

                                diff = damage2 - armor;

                                if (diff >= 0)
                                {
                                    damage2 = diff;

                                    armor = 0;
                                }
                                else
                                {
                                    damage2 = 0;

                                    armor = -diff;
                                }
                            
                                if (damage1 + damage2 <= 0)
                                {
                                    blockType = BlockType.Armor;
                                }
                                else
                                {
                                    // Hit!
                                }
                            }
                        }
                    }

                    return (damage1 + damage2, blockType);
                }
                else
                {
                    BlockType blockType = BlockType.None;

                    int damage = Context.Current.Server.Randomization.Take(Min, Max);

                    if (monster.Metadata.Immunities.Contains(DamageType) )
                    {
                        damage = 0;
                    }
                
                    if (damage <= 0)
                    {
                        blockType = BlockType.Immune;
                    }
                    else
                    {
                        if (monster.Metadata.Mitigation > 0)
                        {
                            double mitigationPercent = (100 - monster.Metadata.Mitigation) / 100.0;

                            damage = Math.Max(0, (int)(damage * mitigationPercent) );
                        }
                        else
                        {
                            int defense = Formula.DefenseFormula(monster.Metadata.Defense, FightMode.Balanced);

                            int diff = damage - defense;

                            if (diff >= 0)
                            {
                                damage = diff;

                                defense = 0;
                            }
                            else
                            {
                                damage = 0;

                                defense = -diff;
                            }
                        }

                        if (damage <= 0)
                        {
                            blockType = BlockType.Shield;
                        }
                        else
                        {
                            double elementPercent;

                            if ( !monster.Metadata.DamageTakenFromElements.TryGetValue(DamageType, out elementPercent) )
                            {
                                elementPercent = 1;
                            }

                            damage = Math.Max(0, (int)(damage * elementPercent) );

                            if (damage <= 0)
                            {
                                blockType = BlockType.Armor;
                            }
                            else
                            {
                                int armor = Formula.ArmorFormula(monster.Metadata.Armor);

                                int diff = damage - armor;

                                if (diff >= 0)
                                {
                                    damage = diff;

                                    armor = 0;
                                }
                                else
                                {
                                    damage = 0;

                                    armor = -diff;
                                }
                                                            
                                if (damage <= 0)
                                {
                                    blockType = BlockType.Armor;
                                }
                                else
                                {
                                    // Hit!
                                }
                            }
                        }
                    }

                    return (damage, blockType);
                }
            }

            if (target is Player player)
            {
                double attackPercent;

                if (attacker == null)
                {
                    // environment

                    attackPercent = 1;
                }
                else
                {
                    if (attacker is Monster)
                    {
                        // mvp

                        attackPercent = 1;
                    }
                    else
                    {
                        // pvp

                        if (player.Combat.GetSkullIcon(null) == SkullIcon.Black)
                        {
                            attackPercent = 1;
                        }
                        else
                        {
                            attackPercent = 0.5;
                        }
                    }
                }

                if (AttackModifierDamageType != null && AttackModifierMin != null && AttackModifierMax != null)
                {
                    BlockType blockType = BlockType.None;

                    int damage1 = (int)(Context.Current.Server.Randomization.Take(Min, Max) * attackPercent);

                    int damage2 = (int)(Context.Current.Server.Randomization.Take(AttackModifierMin.Value, AttackModifierMax.Value) * attackPercent);

                    if (damage1 + damage2 <= 0)
                    {
                        
                    }
                    else
                    {
                        int defense = Formula.DefenseFormula(player.Inventory.GetDefense(), player.Client.FightMode);

                        int diff = damage1 - defense;

                        if (diff >= 0)
                        {
                            damage1 = diff;

                            defense = 0;
                        }
                        else
                        {
                            damage1 = 0;

                            defense = -diff;
                        }

                        diff = damage2 - defense;

                        if (diff >= 0)
                        {
                            damage2 = diff;

                            defense = 0;
                        }
                        else
                        {
                            damage2 = 0;

                            defense = -diff;
                        }

                        if (damage1 + damage2 <= 0)
                        {
                            blockType = BlockType.Shield;
                        }
                        else
                        {
                            double armorReductionPercent = player.Inventory.GetArmorReductionPercent(DamageType);

                            damage1 = Math.Max(0, (int)(damage1 * armorReductionPercent) );

                            armorReductionPercent = player.Inventory.GetArmorReductionPercent(AttackModifierDamageType.Value);

                            damage2 = Math.Max(0, (int)(damage2 * armorReductionPercent) );

                            if (damage1 + damage2 <= 0)
                            {
                                blockType = BlockType.Armor;
                            }
                            else
                            {
                                int armor = Formula.ArmorFormula(player.Inventory.GetArmor() );

                                diff = damage1 - armor;

                                if (diff >= 0)
                                {
                                    damage1 = diff;

                                    armor = 0;
                                }
                                else
                                {
                                    damage1 = 0;

                                    armor = -diff;
                                }

                                diff = damage2 - armor;

                                if (diff >= 0)
                                {
                                    damage2 = diff;

                                    armor = 0;
                                }
                                else
                                {
                                    damage2 = 0;

                                    armor = -diff;
                                }
                            
                                if (damage1 + damage2 <= 0)
                                {
                                    blockType = BlockType.Armor;
                                }
                                else
                                {
                                    // Hit!
                                }
                            }
                        }
                    }

                    return (damage1 + damage2, blockType);
                }
                else
                {
                    BlockType blockType = BlockType.None;

                    int damage = (int)(Context.Current.Server.Randomization.Take(Min, Max) * attackPercent);

                    if (damage <= 0)
                    {
                        
                    }
                    else
                    {
                        int defense = Formula.DefenseFormula(player.Inventory.GetDefense(), player.Client.FightMode);

                        int diff = damage - defense;

                        if (diff >= 0)
                        {
                            damage = diff;

                            defense = 0;
                        }
                        else
                        {
                            damage = 0;

                            defense = -diff;
                        }

                        if (damage <= 0)
                        {
                            blockType = BlockType.Shield;
                        }
                        else
                        {
                            double armorReductionPercent = player.Inventory.GetArmorReductionPercent(DamageType);

                            damage = Math.Max(0, (int)(damage * armorReductionPercent) );

                            if (damage <= 0)
                            {
                                blockType = BlockType.Armor;
                            }
                            else
                            {
                                int armor = Formula.ArmorFormula(player.Inventory.GetArmor() );

                                diff = damage - armor;

                                if (diff >= 0)
                                {
                                    damage = diff;

                                    armor = 0;
                                }
                                else
                                {
                                    damage = 0;

                                    armor = -diff;
                                }
                                                            
                                if (damage <= 0)
                                {
                                    blockType = BlockType.Armor;
                                }
                                else
                                {
                                    // Hit!
                                }
                            }
                        }
                    }

                    return (damage, blockType);
                }
            }

            return (0, BlockType.None);
        }

        public abstract Promise Missed(Creature attacker, Creature target, BlockType blockType);

        public abstract Promise Hit(Creature attacker, Creature target, int damage);
    }    
}