using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class CombatAttackCreatureBuilder
    {
        private Creature attacker;

        public CombatAttackCreatureBuilder WithAttacker(Creature attacker)
        {
            this.attacker = attacker;

            return this;
        }

        private Creature target;

        public CombatAttackCreatureBuilder WithTarget(Creature target)
        {
            this.target = target;

            return this;
        }

        private ProjectileType? projectileType;

        public CombatAttackCreatureBuilder WithProjectileType(ProjectileType? projectileType)
        {
            this.projectileType = projectileType;

            return this;
        }

        private MagicEffectType? magicEffectType;

        public CombatAttackCreatureBuilder WithMagicEffectType(MagicEffectType? magicEffectType)
        {
            this.magicEffectType = magicEffectType;

            return this;
        }

        private MagicEffectType? missedMagicEffectType;

        public CombatAttackCreatureBuilder WithMissedMagicEffectType(MagicEffectType? missedMagicEffectType)
        {
            this.missedMagicEffectType = missedMagicEffectType;

            return this;
        }

        private MagicEffectType? damageMagicEffectType;

        public CombatAttackCreatureBuilder WithDamageMagicEffectType(MagicEffectType? damageMagicEffectType)
        {
            this.damageMagicEffectType = damageMagicEffectType;

            return this;
        }
        
        private AnimatedTextColor? animatedTextColor;

        public CombatAttackCreatureBuilder WithAnimatedTextColor(AnimatedTextColor? animatedTextColor)
        {
            this.animatedTextColor = animatedTextColor;

            return this;
        }

        private Func<Creature, Creature, int> formula;

        public CombatAttackCreatureBuilder WithFormula(Func<Creature, Creature, int> formula)
        {
            this.formula = formula;

            return this;
        }

        public async Promise Build()
        {
            if (projectileType != null)
            {
                await Context.Current.AddCommand(new ShowProjectileCommand(attacker.Tile.Position, target.Tile.Position, projectileType.Value) );
            }

            if (magicEffectType != null)
            {
                await Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, magicEffectType.Value) );
            }

            int damage = formula(attacker, target);

            if (attacker != target || damage > 0)
            {
                if (target is Player player)
                {
                    if (attacker != null)
                    {
                        if (damage <= 0)
                        {
                            Context.Current.AddPacket(player.Client.Connection, new SetFrameColorOutgoingPacket(attacker.Id, FrameColor.Black) );
                        }

                        if (damage < 0)
                        {
                            Context.Current.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + -damage + " hitpoints due to an attack by " + attacker.Name + ".") );
                        }
                    }
                    else
                    {
                        if (damage < 0)
                        {
                            Context.Current.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + -damage + " hitpoints.") );
                        }
                    }
                }

                if (damage == 0)
                {
                    if (missedMagicEffectType != null)
                    {
                        await Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, missedMagicEffectType.Value) );
                    }
                }
                else
                {
                    if (damage < 0)
                    {
                        if (damageMagicEffectType != null)
                        {
                            await Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, damageMagicEffectType.Value) );
                        }

                        if (animatedTextColor != null)
                        {
                            await Context.Current.AddCommand(new ShowAnimatedTextCommand(target.Tile.Position, animatedTextColor.Value, (-damage).ToString() ) );
                        }
                    }

                    await Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health + damage) );
                }  
            }
        }
    }
}