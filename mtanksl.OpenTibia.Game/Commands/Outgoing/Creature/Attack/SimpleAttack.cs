using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class SimpleAttack : Attack
    {
        public SimpleAttack(ProjectileType? projectileType, MagicEffectType? magicEffectType, AnimatedTextColor? animatedTextColor, int min, int max)
        {
            ProjectileType = projectileType;

            MagicEffectType = magicEffectType;

            AnimatedTextColor = animatedTextColor;

            Min = min;

            Max = max;
        }

        public ProjectileType? ProjectileType { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public AnimatedTextColor? AnimatedTextColor { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public override int Calculate(Creature attacker, Creature target)
        {
            return -Context.Current.Server.Randomization.Take(Min, Max);
        }

        public override Promise Missed(Creature attacker, Creature target)
        {
            return Promise.Completed;
        }

        public override async Promise Hit(Creature attacker, Creature target, int damage)
        {
            if (ProjectileType != null)
            {
                await Context.Current.AddCommand(new ShowProjectileCommand(attacker, target, ProjectileType.Value) );
            }

            if (MagicEffectType != null)
            {
                await Context.Current.AddCommand(new ShowMagicEffectCommand(target, MagicEffectType.Value) );
            }

            if ( !(attacker is Player && (target is Player || target is Npc) ) )
            {
                if (AnimatedTextColor != null)
                {
                    await Context.Current.AddCommand(new ShowAnimatedTextCommand(target, AnimatedTextColor.Value, (-damage).ToString() ) );
                }

                if (target is Player player)
                {
                    if (attacker == null)
                    {
                        Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + -damage + " hitpoints.") );
                    }
                    else
                    {
                        Context.Current.AddPacket(player, new SetFrameColorOutgoingPacket(attacker.Id, FrameColor.Black) );

                        Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + -damage + " hitpoints due to an attack by " + attacker.Name + ".") );
                    }
                }

                await Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health + damage) );
            }
        }
    }
}