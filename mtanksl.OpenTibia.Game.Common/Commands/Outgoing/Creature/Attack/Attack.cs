using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public abstract class Attack
    {
        protected Attack(DamageType damageType, int min, int max)
        {
            DamageType = damageType;

            Min = min;

            Max = max;
        }

        public DamageType DamageType { get; }

        public int Min { get; }

        public int Max { get; }

        public virtual (int Damage, BlockType BlockType) Calculate(Creature attacker, Creature target)
        {
            if (target is Monster monster)
            {
                // pvm or mvm

                int damage = Context.Current.Server.Randomization.Take(Min, Max);

                BlockType blockType = BlockType.None;

                if (damage > 0)
                {
                    double elementPercent;

                    if ( !monster.Metadata.DamageTakenFromElements.TryGetValue(DamageType, out elementPercent) )
                    {
                        elementPercent = 1;
                    }

                    damage = (int)(damage * elementPercent);

                    if (damage <= 0)
                    {
                        damage = 0;

                        blockType = BlockType.Immune;
                    }
                }

                if (damage > 0)
                {
                    int defense = Formula.DefenseFormula(monster.Metadata.Defense, FightMode.Balanced);

                    damage -= defense;

                    if (damage <= 0)
                    {
                        damage = 0;

                        blockType = BlockType.Shield;
                    }
                }

                if (damage > 0)
                {
                    int armor = Formula.ArmorFormula(monster.Metadata.Armor);

                    damage -= armor;

                    if (damage <= 0)
                    {
                        damage = 0;

                        blockType = BlockType.Armor;
                    }
                }

                return (damage, blockType);
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

                int damage = (int)(Context.Current.Server.Randomization.Take(Min, Max) * attackPercent);

                BlockType blockType = BlockType.None;

                if (damage > 0)
                {
                    int defense = Formula.DefenseFormula(player.Inventory.GetDefense(), player.Client.FightMode);

                    damage -= defense;

                    if (damage <= 0)
                    {
                        damage = 0;

                        blockType = BlockType.Shield;
                    }
                }

                if (damage > 0)
                {
                    double armorReductionPercent = player.Inventory.GetArmorReductionPercent(DamageType);

                    damage = (int)(damage * armorReductionPercent);

                    if (damage <= 0)
                    {
                        damage = 0;

                        blockType = BlockType.Armor;
                    }
                }

                if (damage > 0)
                {
                    int armor = Formula.ArmorFormula(player.Inventory.GetArmor() );

                    damage -= armor;

                    if (damage <= 0)
                    {
                        damage = 0;

                        blockType = BlockType.Armor;
                    }
                }

                return (damage, blockType);
            }

            throw new NotImplementedException();
        }

        public abstract Promise Missed(Creature attacker, Creature target, BlockType blockType);

        public abstract Promise Hit(Creature attacker, Creature target, int damage);
    }    
}