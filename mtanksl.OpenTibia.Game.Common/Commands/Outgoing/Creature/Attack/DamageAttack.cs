using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class DamageAttack : Attack
    {
        private ProjectileType? projectileType;

        private MagicEffectType? magicEffectType;

        private DamageType damageType;

        private int min;

        private int max;

        private DamageType? attackModifierDamageType;

        private int? attackModifierMin;

        private int? attackModifierMax;

        private bool blockable;

        public DamageAttack(ProjectileType? projectileType, MagicEffectType? magicEffectType, DamageType damageType, int min, int max, bool blockable) 
            
            : this(projectileType, magicEffectType, damageType, min, max, null, null, null, blockable)
        {

        }

        public DamageAttack(ProjectileType? projectileType, MagicEffectType? magicEffectType, DamageType damageType, int min, int max, DamageType? attackModifierDamageType, int? attackModifierMin, int? attackModifierMax, bool blockable)
        {
            this.projectileType = projectileType;

            this.magicEffectType = magicEffectType;

            this.damageType = damageType;

            this.min = min;

            this.max = max;

            this.attackModifierDamageType = attackModifierDamageType;

            this.attackModifierMin = attackModifierMin;

            this.attackModifierMax = attackModifierMax;

            this.blockable = blockable;
        }

        public override (int Damage, BlockType BlockType, HashSet<Item> RemoveCharges) Calculate(Creature attacker, Creature target)
        {
            if (target is Monster monster)
            {
                BlockType blockType = BlockType.None;

                int damage1 = Context.Current.Server.Randomization.Take(min, max);

                int damage2 = attackModifierDamageType != null ? Context.Current.Server.Randomization.Take(attackModifierMin.Value, attackModifierMax.Value): 0;
                                
                if (monster.Metadata.Immunities.Contains(damageType) )
                {
                    damage1 = 0;
                }
                
                if (attackModifierDamageType != null && monster.Metadata.Immunities.Contains(attackModifierDamageType.Value) )
                {
                    damage2 = 0;
                }

                if (damage1 + damage2 <= 0)
                {
                    blockType = BlockType.Immune;
                }
                else
                {
                    if (blockable)
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

                            if ( !monster.Metadata.DamageTakenFromElements.TryGetValue(damageType, out elementPercent) )
                            {
                                elementPercent = 1;
                            }

                            damage1 = Math.Max(0, (int)(damage1 * elementPercent) );

                            if (attackModifierDamageType != null && !monster.Metadata.DamageTakenFromElements.TryGetValue(attackModifierDamageType.Value, out elementPercent) )
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
                }

                return (damage1 + damage2, blockType, null);
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

                BlockType blockType = BlockType.None;

                HashSet<Item> removeCharges = null;

                int damage1 = (int)(Context.Current.Server.Randomization.Take(min, max) * attackPercent);

                int damage2 = attackModifierDamageType != null ? (int)(Context.Current.Server.Randomization.Take(attackModifierMin.Value, attackModifierMax.Value) * attackPercent) : 0;

                if (blockable)
                {
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
                            double armorReductionPercent = player.Inventory.GetArmorReductionPercent(damageType, ref removeCharges);

                            damage1 = Math.Max(0, (int)(damage1 * armorReductionPercent) );

                            if (attackModifierDamageType != null)
                            {
                                armorReductionPercent = player.Inventory.GetArmorReductionPercent(attackModifierDamageType.Value, ref removeCharges);

                                damage2 = Math.Max(0, (int)(damage2 * armorReductionPercent) );
                            }

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
                }

                return (damage1 + damage2, blockType, removeCharges);
            }

            return (0, BlockType.None, null);
        }

        public override async Promise NoDamage(Creature attacker, Creature target, BlockType blockType)
        {
            if (projectileType != null)
            {
                await Context.Current.AddCommand(new ShowProjectileCommand(attacker, target, projectileType.Value) );
            }

            if (blockType == BlockType.Armor)
            {
                await Context.Current.AddCommand(new ShowMagicEffectCommand(target, MagicEffectType.YellowSpark) );
            }
            else
            {
                await Context.Current.AddCommand(new ShowMagicEffectCommand(target, MagicEffectType.Puff) );
            }

            if (target != attacker)
            {
                if (target is Player player)
                {
                    if (attacker != null)
                    {
                        Context.Current.AddPacket(player, new SetFrameColorOutgoingPacket(attacker.Id, FrameColor.Black) );
                    }

                    if ( (blockType == BlockType.Shield || blockType == BlockType.Armor) && Formula.GetShield(player) != null)
                    {
                        await Context.Current.AddCommand(new PlayerAddSkillPointsCommand(player, Skill.Shield, 1) );
                    }
                }
            }
        }

        public override async Promise Damage(Creature attacker, Creature target, int damage)
        {
            if (projectileType != null)
            {
                await Context.Current.AddCommand(new ShowProjectileCommand(attacker, target, projectileType.Value) );
            }

            if (target != attacker)
            {
                if (target is Player player)
                {      
                    int manaDamage = 0;

                    int healthDamage = 0;

                    if (damageType == DamageType.ManaDrain)
                    {
                        manaDamage = damage;

                        healthDamage = 0;
                    }
                    else if (damageType == DamageType.LifeDrain)
                    {
                        manaDamage = 0;

                        healthDamage = damage;
                    }
                    else
                    {
                        if (player.HasSpecialCondition(SpecialCondition.MagicShield) )
                        {
                            manaDamage = Math.Min(player.Mana, damage);

                            healthDamage = damage - manaDamage;
                        }
                        else
                        {
                            manaDamage = 0;

                            healthDamage = damage;
                        }
                    }

                    if (attacker != null)
                    {
                        Context.Current.Server.Combats.AddHitToTarget(attacker, player, damage);

                        Context.Current.AddPacket(player, new SetFrameColorOutgoingPacket(attacker.Id, FrameColor.Black) );
                    }

                    if (manaDamage > 0)
                    {
                        if (attacker != null)
                        {
                            Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(MessageMode.Status, "You lose " + manaDamage + " mana due to an attack by " + attacker.Name + ".") );
                        }
                        else
                        {
                            Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(MessageMode.Status, "You lose " + manaDamage + " mana.") );
                        }

                        await Context.Current.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.BlueRings) );

                        await Context.Current.AddCommand(new ShowAnimatedTextCommand(player, AnimatedTextColor.Blue, manaDamage.ToString() ) );

                        await Context.Current.AddCommand(new PlayerUpdateManaCommand(player, player.Mana - manaDamage) );
                    }

                    if (healthDamage > 0)
                    {
                        if (attacker != null)
                        {
                            Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(MessageMode.Status, "You lose " + healthDamage + " hitpoints due to an attack by " + attacker.Name + ".") );
                        }
                        else
                        {
                            Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(MessageMode.Status, "You lose " + healthDamage + " hitpoints.") );
                        }

                        MagicEffectType? magicEffectType = this.magicEffectType ?? damageType.ToMagicEffectType(Race.Blood);

                        if (magicEffectType != null)
                        {
                            await Context.Current.AddCommand(new ShowMagicEffectCommand(player, magicEffectType.Value) );
                        }

                        AnimatedTextColor? animatedTextColor = damageType.ToAnimatedTextColor(Race.Blood);

                        if (animatedTextColor != null)
                        {
                            await Context.Current.AddCommand(new ShowAnimatedTextCommand(player, animatedTextColor.Value, healthDamage.ToString() ) );
                        }

                        await Context.Current.AddCommand(new CreatureUpdateHealthCommand(player, player.Health - healthDamage) );
                    }
                }
                else if (target is Monster monster)
                {
                    if (monster.Invisible)
                    {
                        await Context.Current.AddCommand(new CreatureRemoveConditionCommand(monster, ConditionSpecialCondition.Invisible) );
                    }

                    if (attacker != null)
                    {
                        Context.Current.Server.Combats.AddHitToTarget(attacker, monster, damage);


                    }
                                                             
                    MagicEffectType? magicEffectType = this.magicEffectType ?? damageType.ToMagicEffectType(monster.Metadata.Race);
                   
                    if (magicEffectType != null)
                    {
                        await Context.Current.AddCommand(new ShowMagicEffectCommand(monster, magicEffectType.Value) );
                    }

                    AnimatedTextColor? animatedTextColor = damageType.ToAnimatedTextColor(monster.Metadata.Race);

                    if (animatedTextColor != null)
                    {
                        await Context.Current.AddCommand(new ShowAnimatedTextCommand(monster, animatedTextColor.Value, damage.ToString() ) );
                    }

                    await Context.Current.AddCommand(new CreatureUpdateHealthCommand(monster, monster.Health - damage) );
                }
            }
            else
            {
                if (target is Player player)
                {
                    MagicEffectType? magicEffectType = this.magicEffectType ?? damageType.ToMagicEffectType(Race.Blood);

                    if (magicEffectType != null)
                    {
                        await Context.Current.AddCommand(new ShowMagicEffectCommand(player, magicEffectType.Value) );
                    }
                }
                else if (target is Monster monster)
                {
                    MagicEffectType? magicEffectType = this.magicEffectType ?? damageType.ToMagicEffectType(monster.Metadata.Race);
                   
                    if (magicEffectType != null)
                    {
                        await Context.Current.AddCommand(new ShowMagicEffectCommand(monster, magicEffectType.Value) );
                    }
                }
            }
        }
    }
}