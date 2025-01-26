using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class DamageAttack : Attack
    {
        protected ProjectileType? projectileType;

        private MagicEffectType? magicEffectType;

        public DamageAttack(ProjectileType? projectileType, MagicEffectType? magicEffectType, DamageType damageType, int min, int max) : base(damageType, min, max)
        {
            this.projectileType = projectileType;

            this.magicEffectType = magicEffectType;
        }

        public override async Promise Missed(Creature attacker, Creature target)
        {
            if (projectileType != null)
            {
                await Context.Current.AddCommand(new ShowProjectileCommand(attacker, target, projectileType.Value) );
            }

            await Context.Current.AddCommand(new ShowMagicEffectCommand(target, MagicEffectType.Puff) );

            if (target != attacker)
            {
                if (target is Player player)
                {
                    if (attacker != null)
                    {
                        Context.Current.AddPacket(player, new SetFrameColorOutgoingPacket(attacker.Id, FrameColor.Black) );
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
                if (target is Player targetPlayer)
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
                        CreatureConditionBehaviour creatureConditionBehaviour = Context.Current.Server.GameObjectComponents.GetComponents<CreatureConditionBehaviour>(target)
                            .Where(c => c.Condition.ConditionSpecialCondition == ConditionSpecialCondition.MagicShield)
                            .FirstOrDefault();

                        if (creatureConditionBehaviour != null)
                        {
                            manaDamage = Math.Min(targetPlayer.Mana, damage);

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
                        ICombatCollection combats = Context.Current.Server.Combats;

                        combats.AddHitToTarget(attacker, target, damage);

                        if (attacker is Player attackerPlayer)
                        {
                            if ( !combats.WhiteSkullContains(attackerPlayer) ) // If an attacker without white skull
                            {
                                if ( !combats.WhiteSkullContains(targetPlayer) ) // Attacks a target without white skull
                                {
                                    combats.WhiteSkullAdd(attackerPlayer, targetPlayer); // Makes attacker white skull, and add target to self defense list

                                    foreach (var observer in Context.Current.Server.Map.GetObserversOfTypePlayer(attackerPlayer.Tile.Position) )
                                    {
                                        byte clientIndex;

                                        if (observer.Client.TryGetIndex(attackerPlayer, out clientIndex) )
                                        {
                                            Context.Current.AddPacket(observer, new SetSkullIconOutgoingPacket(attackerPlayer.Id, SkullIcon.White) );
                                        }
                                    }
                                }
                                else // Attacks a target with white skull
                                {
                                    if ( !combats.WhiteSkullContains(targetPlayer, attackerPlayer) ) // If it is not self defense
                                    {
                                        combats.YellowSkullAdd(attackerPlayer, targetPlayer); // Makes attacjer yellow skull

                                        Context.Current.AddPacket(targetPlayer, new SetSkullIconOutgoingPacket(attackerPlayer.Id, SkullIcon.Yellow) );
                                    }
                                }
                            }
                            else // If an attacker with white skull
                            {
                                if ( !combats.WhiteSkullContains(targetPlayer) && 
                                     !combats.WhiteSkullContains(attackerPlayer, targetPlayer) && 
                                     !combats.YellowSkullContains(targetPlayer, attackerPlayer) ) // Attacks a new target without white skull
                                {
                                    combats.WhiteSkullAdd(attackerPlayer, targetPlayer); // Add target to self defense list
                                }
                            }
                        }

                        Context.Current.AddPacket(targetPlayer, new SetFrameColorOutgoingPacket(attacker.Id, FrameColor.Black) );
                    }

                    if (manaDamage > 0)
                    {
                        if (attacker != null)
                        {
                            Context.Current.AddPacket(targetPlayer, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + manaDamage + " mana due to an attack by " + attacker.Name + ".") );
                        }
                        else
                        {
                            Context.Current.AddPacket(targetPlayer, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + manaDamage + " mana.") );
                        }

                        await Context.Current.AddCommand(new ShowMagicEffectCommand(target, MagicEffectType.BlueRings) );

                        await Context.Current.AddCommand(new ShowAnimatedTextCommand(target, AnimatedTextColor.Blue, manaDamage.ToString() ) );

                        await Context.Current.AddCommand(new PlayerUpdateManaCommand(targetPlayer, targetPlayer.Mana - manaDamage) );
                    }

                    if (healthDamage > 0)
                    {
                        if (attacker != null)
                        {
                            Context.Current.AddPacket(targetPlayer, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + healthDamage + " hitpoints due to an attack by " + attacker.Name + ".") );
                        }
                        else
                        {
                            Context.Current.AddPacket(targetPlayer, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + healthDamage + " hitpoints.") );
                        }

                        MagicEffectType? magicEffectType = this.magicEffectType ?? DamageType.ToMagicEffectType(Race.Blood);

                        if (magicEffectType != null)
                        {
                            await Context.Current.AddCommand(new ShowMagicEffectCommand(target, magicEffectType.Value) );
                        }

                        AnimatedTextColor? animatedTextColor = DamageType.ToAnimatedTextColor(Race.Blood);

                        if (animatedTextColor != null)
                        {
                            await Context.Current.AddCommand(new ShowAnimatedTextCommand(target, animatedTextColor.Value, healthDamage.ToString() ) );
                        }

                        await Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health - healthDamage) );
                    }
                }
                else if (target is Monster monster)
                {
                    if (attacker != null)
                    {
                        Context.Current.Server.Combats.AddHitToTarget(attacker, target, damage);


                    }
                                                             
                    MagicEffectType? magicEffectType = this.magicEffectType ?? DamageType.ToMagicEffectType(monster.Metadata.Race);
                   
                    if (magicEffectType != null)
                    {
                        await Context.Current.AddCommand(new ShowMagicEffectCommand(target, magicEffectType.Value) );
                    }

                    AnimatedTextColor? animatedTextColor = DamageType.ToAnimatedTextColor(monster.Metadata.Race);

                    if (animatedTextColor != null)
                    {
                        await Context.Current.AddCommand(new ShowAnimatedTextCommand(target, animatedTextColor.Value, damage.ToString() ) );
                    }

                    await Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health - damage) );
                }
            }
            else
            {
                if (target is Player player)
                {
                    MagicEffectType? magicEffectType = this.magicEffectType ?? DamageType.ToMagicEffectType(Race.Blood);

                    if (magicEffectType != null)
                    {
                        await Context.Current.AddCommand(new ShowMagicEffectCommand(target, magicEffectType.Value) );
                    }
                }
                else if (target is Monster monster)
                {
                    MagicEffectType? magicEffectType = this.magicEffectType ?? DamageType.ToMagicEffectType(monster.Metadata.Race);
                   
                    if (magicEffectType != null)
                    {
                        await Context.Current.AddCommand(new ShowMagicEffectCommand(target, magicEffectType.Value) );
                    }
                }
            }
        }
    }
}