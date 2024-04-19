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
        public SimpleAttack(ProjectileType? projectileType, MagicEffectType? magicEffectType, AnimatedTextColor? animatedTextColor, int min, int max)
        {
            ShowProjectileType = projectileType;

            ShowMagicEffectType = magicEffectType;

            ShowAnimatedTextColor = animatedTextColor;

            Min = min;

            Max = max;
        }

        public ProjectileType? ShowProjectileType { get; set; }

        public MagicEffectType? ShowMagicEffectType { get; set; }

        public AnimatedTextColor? ShowAnimatedTextColor { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public override int Calculate(Creature attacker, Creature target)
        {
            return Context.Current.Server.Randomization.Take(Min, Max);
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
            if (ShowProjectileType != null)
            {
                await Context.Current.AddCommand(new ShowProjectileCommand(attacker, target, ShowProjectileType.Value) );
            }

            if (target is Player player)
            {
                CreatureConditionBehaviour creatureConditionBehaviour = Context.Current.Server.GameObjectComponents.GetComponents<CreatureConditionBehaviour>(target).Where(c => c.Condition.ConditionSpecialCondition == ConditionSpecialCondition.MagicShield).FirstOrDefault();

                if (creatureConditionBehaviour != null && player.Mana > 0)
                {
                    int mana = Math.Min(damage, player.Mana);

                    await Context.Current.AddCommand(new ShowMagicEffectCommand(target, MagicEffectType.BlueRings) );

                    await Context.Current.AddCommand(new ShowAnimatedTextCommand(target, AnimatedTextColor.Blue, damage.ToString() ) );

                    if (attacker != null)
                    {                   
                        Context.Current.AddPacket(player, new SetFrameColorOutgoingPacket(attacker.Id, FrameColor.Black) );

                        Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + mana + " mana due to an attack by " + attacker.Name + ".") );
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

                if (ShowMagicEffectType != null)
                {
                    await Context.Current.AddCommand(new ShowMagicEffectCommand(target, ShowMagicEffectType.Value) );
                }

                if (ShowAnimatedTextColor != null)
                {
                    await Context.Current.AddCommand(new ShowAnimatedTextCommand(target, ShowAnimatedTextColor.Value, damage.ToString() ) );
                }

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
            else
            {
                if (ShowMagicEffectType != null)
                {
                    await Context.Current.AddCommand(new ShowMagicEffectCommand(target, ShowMagicEffectType.Value) );
                }

                if (ShowAnimatedTextColor != null)
                {
                    await Context.Current.AddCommand(new ShowAnimatedTextCommand(target, ShowAnimatedTextColor.Value, damage.ToString() ) );
                }                
            }

            await Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health - damage) );
        }
    }
}