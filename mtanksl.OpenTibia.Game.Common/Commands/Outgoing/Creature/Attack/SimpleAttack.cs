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

        private int min;

        private int max;

        public SimpleAttack(ProjectileType? projectileType, MagicEffectType? magicEffectType, AnimatedTextColor? animatedTextColor, int min, int max)
        {
            this.projectileType = projectileType;

            this.magicEffectType = magicEffectType;

            this.animatedTextColor = animatedTextColor;

            this.min = min;

            this.max = max;
        }

        public override int Calculate(Creature attacker, Creature target)
        {
            return Context.Current.Server.Randomization.Take(min, max);
        }

        public override async Promise Missed(Creature attacker, Creature target)
        {
            await Context.Current.AddCommand(new ShowMagicEffectCommand(target, MagicEffectType.Puff) );

            if (target is Player player)
            {
                if (attacker != null)
                {
                    Context.Current.AddPacket(player, new SetFrameColorOutgoingPacket(attacker.Id, FrameColor.Black) );
                }
            }
        }

        public override async Promise Hit(Creature attacker, Creature target, int damage)
        {
            if (projectileType != null)
            {
                await Context.Current.AddCommand(new ShowProjectileCommand(attacker, target, projectileType.Value) );
            }

            if (target is Player player)
            {
                CreatureConditionBehaviour creatureConditionBehaviour = Context.Current.Server.GameObjectComponents.GetComponents<CreatureConditionBehaviour>(target)
                    .Where(c => c.Condition.ConditionSpecialCondition == ConditionSpecialCondition.MagicShield)
                    .FirstOrDefault();

                if (creatureConditionBehaviour != null && player.Mana > 0)
                {
                    int mana = Math.Min(damage, player.Mana);

                    await Context.Current.AddCommand(new ShowMagicEffectCommand(target, MagicEffectType.BlueRings) );

                    await Context.Current.AddCommand(new ShowAnimatedTextCommand(target, AnimatedTextColor.Blue, damage.ToString() ) );

                    if (attacker != null)
                    {                   
                        Context.Current.AddPacket(player, new SetFrameColorOutgoingPacket(attacker.Id, FrameColor.Black) );

                        if (target == attacker)
                        {
                            Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + mana + " mana due to own attack.") );
                        }
                        else
                        {
                            Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + mana + " mana due to an attack by " + attacker.Name + ".") );
                        }
                    }
                    else
                    {
                        Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + mana + " mana.") );
                    }

                    await Context.Current.AddCommand(new PlayerUpdateManaCommand(player, player.Mana - mana) );

                    damage -= mana;

                    if (damage == 0)
                    {
                        return;
                    }
                }

                if (magicEffectType != null)
                {
                    await Context.Current.AddCommand(new ShowMagicEffectCommand(target, magicEffectType.Value) );
                }

                if (animatedTextColor != null)
                {
                    await Context.Current.AddCommand(new ShowAnimatedTextCommand(target, animatedTextColor.Value, damage.ToString() ) );
                }

                if (attacker != null)
                {
                    Context.Current.AddPacket(player, new SetFrameColorOutgoingPacket(attacker.Id, FrameColor.Black) );

                    if (target == attacker)
                    {
                        Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + damage + " hitpoints due to own attack.") );
                    }
                    else
                    {
                        Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + damage + " hitpoints due to an attack by " + attacker.Name + ".") );
                    }
                }
                else
                {
                    Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + damage + " hitpoints.") );
                }
            }
            else
            {
                if (magicEffectType != null)
                {
                    await Context.Current.AddCommand(new ShowMagicEffectCommand(target, magicEffectType.Value) );
                }

                if (animatedTextColor != null)
                {
                    await Context.Current.AddCommand(new ShowAnimatedTextCommand(target, animatedTextColor.Value, damage.ToString() ) );
                }                
            }

            await Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health - damage) );
        }
    }
}