using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class DamageAttack : Attack
    {
        protected ProjectileType? projectileType;

        private MagicEffectType? magicEffectType;

        public DamageAttack(ProjectileType? projectileType, MagicEffectType? magicEffectType, DamageType damageType, int min, int max) 
            
            : this(projectileType, magicEffectType, damageType, min, max, null, null, null)
        {

        }

        public DamageAttack(ProjectileType? projectileType, MagicEffectType? magicEffectType, DamageType damageType, int min, int max, DamageType? attackModifierDamageType, int? attackModifierMin, int? attackModifierMax) 
            
            : base(damageType, min, max, attackModifierDamageType, attackModifierMin, attackModifierMax)
        {
            this.projectileType = projectileType;

            this.magicEffectType = magicEffectType;
        }

        public override async Promise Missed(Creature attacker, Creature target, BlockType blockType)
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

                        if ( (blockType == BlockType.Shield || blockType == BlockType.Armor) && Formula.GetShield(player) != null)
                        {
                            await Context.Current.AddCommand(new PlayerAddSkillPointsCommand(player, Skill.Shield, 1) );
                        }
                    }
                }
            }
        }

        public override async Promise Hit(Creature attacker, Creature target, int damage)
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

                    if (DamageType == DamageType.ManaDrain)
                    {
                        manaDamage = damage;

                        healthDamage = 0;
                    }
                    else if (DamageType == DamageType.LifeDrain)
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
                            Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + manaDamage + " mana due to an attack by " + attacker.Name + ".") );
                        }
                        else
                        {
                            Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + manaDamage + " mana.") );
                        }

                        await Context.Current.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.BlueRings) );

                        await Context.Current.AddCommand(new ShowAnimatedTextCommand(player, AnimatedTextColor.Blue, manaDamage.ToString() ) );

                        await Context.Current.AddCommand(new PlayerUpdateManaCommand(player, player.Mana - manaDamage) );
                    }

                    if (healthDamage > 0)
                    {
                        if (attacker != null)
                        {
                            Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + healthDamage + " hitpoints due to an attack by " + attacker.Name + ".") );
                        }
                        else
                        {
                            Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + healthDamage + " hitpoints.") );
                        }

                        MagicEffectType? magicEffectType = this.magicEffectType ?? DamageType.ToMagicEffectType(Race.Blood);

                        if (magicEffectType != null)
                        {
                            await Context.Current.AddCommand(new ShowMagicEffectCommand(player, magicEffectType.Value) );
                        }

                        AnimatedTextColor? animatedTextColor = DamageType.ToAnimatedTextColor(Race.Blood);

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
                                                             
                    MagicEffectType? magicEffectType = this.magicEffectType ?? DamageType.ToMagicEffectType(monster.Metadata.Race);
                   
                    if (magicEffectType != null)
                    {
                        await Context.Current.AddCommand(new ShowMagicEffectCommand(monster, magicEffectType.Value) );
                    }

                    AnimatedTextColor? animatedTextColor = DamageType.ToAnimatedTextColor(monster.Metadata.Race);

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
                    MagicEffectType? magicEffectType = this.magicEffectType ?? DamageType.ToMagicEffectType(Race.Blood);

                    if (magicEffectType != null)
                    {
                        await Context.Current.AddCommand(new ShowMagicEffectCommand(player, magicEffectType.Value) );
                    }
                }
                else if (target is Monster monster)
                {
                    MagicEffectType? magicEffectType = this.magicEffectType ?? DamageType.ToMagicEffectType(monster.Metadata.Race);
                   
                    if (magicEffectType != null)
                    {
                        await Context.Current.AddCommand(new ShowMagicEffectCommand(monster, magicEffectType.Value) );
                    }
                }
            }
        }
    }
}