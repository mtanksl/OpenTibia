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

        private AnimatedTextColor? animatedTextColor;

        public SimpleAttack(ProjectileType? projectileType, MagicEffectType? magicEffectType, AnimatedTextColor? animatedTextColor, int min, int max) : base(min, max)
        {
            this.projectileType = projectileType;

            this.magicEffectType = magicEffectType;

            this.animatedTextColor = animatedTextColor;
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
                if (attacker != null && target != null && damage > 0)
                {
                    Context.Current.Server.Combats.AddHitToTarget(attacker, target, damage);
                }

                if (target is Player player)
                {
                    CreatureConditionBehaviour creatureConditionBehaviour = Context.Current.Server.GameObjectComponents.GetComponents<CreatureConditionBehaviour>(target)
                        .Where(c => c.Condition.ConditionSpecialCondition == ConditionSpecialCondition.MagicShield)
                        .FirstOrDefault();

                    if (creatureConditionBehaviour != null)
                    {                        
                        int manaDamage = Math.Min(damage, player.Mana);

                        if (manaDamage > 0)
                        {
                            damage -= manaDamage;

                            if (attacker != null)
                            {
                                Context.Current.AddPacket(player, new SetFrameColorOutgoingPacket(attacker.Id, FrameColor.Black) );

                                Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + manaDamage + " mana due to an attack by " + attacker.Name + ".") );
                            }
                            else
                            {
                                Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + manaDamage + " mana.") );
                            }

                            await Context.Current.AddCommand(new PlayerUpdateManaCommand(player, player.Mana - manaDamage) );

                            if (damage == 0)
                            {
                                await Context.Current.AddCommand(new ShowMagicEffectCommand(target, MagicEffectType.BlueRings) );

                                await Context.Current.AddCommand(new ShowAnimatedTextCommand(target, AnimatedTextColor.Blue, manaDamage.ToString() ) );

                                return;
                            }
                        }
                    }
                }
            }

            if (magicEffectType != null)
            {
                await Context.Current.AddCommand(new ShowMagicEffectCommand(target, magicEffectType.Value) );
            }

            if (target != attacker)
            {
                if (target is Player || target is Monster)
                {
                    if (animatedTextColor != null)
                    {
                        await Context.Current.AddCommand(new ShowAnimatedTextCommand(target, animatedTextColor.Value, damage.ToString() ) );
                    }

                    if (target is Player player)
                    {
                        if (attacker != null)
                        {
                            Context.Current.AddPacket(player, new SetFrameColorOutgoingPacket(attacker.Id, FrameColor.Black) );

                            Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + damage + " hitpoints due to an attack by " + attacker.Name + ".") );
                        }
                        else
                        {
                            Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + damage + " hitpoints.") );
                        }
                    }

                    await Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health - damage) );
                }
            }
        }
    }
}