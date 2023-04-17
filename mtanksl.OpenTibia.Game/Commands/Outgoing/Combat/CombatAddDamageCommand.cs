using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class CombatAddDamageCommand : Command
    {
        public CombatAddDamageCommand(Creature attacker, Creature target, Func<Creature, Creature, int> formula, MagicEffectType? missedMagicEffectType, MagicEffectType? damageMagicEffectType, AnimatedTextColor? damageAnimatedTextColor)
        {
            Attacker = attacker;

            Target = target;

            Formula = formula;

            MissedMagicEffectType = missedMagicEffectType;

            DamageMagicEffectType = damageMagicEffectType;

            DamageAnimatedTextColor = damageAnimatedTextColor;
        }

        public Creature Attacker { get; set; }

        public Creature Target { get; set; }

        public Func<Creature, Creature, int> Formula { get; set; }

        public MagicEffectType? MissedMagicEffectType { get; set; }

        public MagicEffectType? DamageMagicEffectType { get; set; }

        public AnimatedTextColor? DamageAnimatedTextColor { get; set; }

        public override async Promise Execute()
        {
            int damage = Formula(Attacker, Target);

            if (Attacker != Target || damage > 0)
            {
                if (Target is Player player)
                {
                    if (Attacker != null)
                    {
                        if (damage <= 0)
                        {
                            Context.Current.AddPacket(player.Client.Connection, new SetFrameColorOutgoingPacket(Attacker.Id, FrameColor.Black) );
                        }

                        if (damage < 0)
                        {
                            Context.Current.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + -damage + " hitpoints due to an attack by " + Attacker.Name + ".") );
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
                    if (MissedMagicEffectType != null)
                    {
                        await Context.Current.AddCommand(new ShowMagicEffectCommand(Target.Tile.Position, MissedMagicEffectType.Value) );
                    }
                }
                else
                {
                    if (damage < 0)
                    {
                        if (DamageMagicEffectType != null)
                        {
                            await Context.Current.AddCommand(new ShowMagicEffectCommand(Target.Tile.Position, DamageMagicEffectType.Value) );
                        }

                        if (DamageAnimatedTextColor != null)
                        {
                            await Context.Current.AddCommand(new ShowAnimatedTextCommand(Target.Tile.Position, DamageAnimatedTextColor.Value, (-damage).ToString() ) );
                        }
                    }

                    await Context.Current.AddCommand(new CreatureUpdateHealthCommand(Target, Target.Health + damage) );
                }
            }
        }
    }
}