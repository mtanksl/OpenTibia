using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class SimpleAttack : Attack
    {
        protected ProjectileType? projectileType;

        private MagicEffectType? magicEffectType;

        public SimpleAttack(ProjectileType? projectileType, MagicEffectType? magicEffectType, DamageType damageType, int min, int max) : base(damageType, min, max)
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
                if (target is Player player)
                {      
                    int manaDamage = 0;

                    CreatureConditionBehaviour creatureConditionBehaviour = Context.Current.Server.GameObjectComponents.GetComponents<CreatureConditionBehaviour>(target)
                        .Where(c => c.Condition.ConditionSpecialCondition == ConditionSpecialCondition.MagicShield)
                        .FirstOrDefault();

                    if (creatureConditionBehaviour != null)
                    {
                        manaDamage = Math.Min(player.Mana, damage);

                        damage -= manaDamage;
                    }

                    if (attacker != null)
                    {
                        Context.Current.Server.Combats.AddHitToTarget(attacker, target, damage);

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

                        await Context.Current.AddCommand(new ShowMagicEffectCommand(target, MagicEffectType.BlueRings) );

                        await Context.Current.AddCommand(new ShowAnimatedTextCommand(target, AnimatedTextColor.Blue, manaDamage.ToString() ) );

                        await Context.Current.AddCommand(new PlayerUpdateManaCommand(player, player.Mana - manaDamage) );
                    }

                    if (damage > 0)
                    {
                        if (attacker != null)
                        {
                            Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + damage + " hitpoints due to an attack by " + attacker.Name + ".") );
                        }
                        else
                        {
                            Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + damage + " hitpoints.") );
                        }

                        if (magicEffectType != null)
                        {
                            await Context.Current.AddCommand(new ShowMagicEffectCommand(target, magicEffectType.Value) );
                        }

                        AnimatedTextColor? animatedTextColor = DamageType.ToAnimatedTextColor();

                        if (animatedTextColor != null)
                        {
                            await Context.Current.AddCommand(new ShowAnimatedTextCommand(target, animatedTextColor.Value, damage.ToString() ) );
                        }

                        await Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health - damage) );
                    }
                }
                else if (target is Monster)
                {
                    if (attacker != null)
                    {
                        Context.Current.Server.Combats.AddHitToTarget(attacker, target, damage);


                    }

                    if (magicEffectType != null)
                    {
                        await Context.Current.AddCommand(new ShowMagicEffectCommand(target, magicEffectType.Value) );
                    }

                    AnimatedTextColor? animatedTextColor = DamageType.ToAnimatedTextColor();

                    if (animatedTextColor != null)
                    {
                        await Context.Current.AddCommand(new ShowAnimatedTextCommand(target, animatedTextColor.Value, damage.ToString() ) );
                    }

                    await Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health - damage) );
                }
            }
        }
    }
}