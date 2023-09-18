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
            if (attacker != target)
            {
                if (target is Player player)
                {
                    if (attacker != null)
                    {
                        Context.Current.AddPacket(player.Client.Connection, new SetFrameColorOutgoingPacket(attacker.Id, FrameColor.Black) );
                    }
                }
            }

            if (ProjectileType != null)
            {
                await Context.Current.AddCommand(new ShowProjectileCommand(attacker.Tile.Position, target.Tile.Position, ProjectileType.Value) );
            }

            if (MagicEffectType != null)
            {
                await Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, MagicEffectType.Value) );
            }

            if (AnimatedTextColor != null)
            {
                if (attacker != target)
                {
                    if ( !(attacker is Player) && target is Player player)
                    {
                        await Context.Current.AddCommand(new ShowAnimatedTextCommand(target.Tile.Position, AnimatedTextColor.Value, (-damage).ToString() ) );

                        if (attacker != null)
                        {
                            Context.Current.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + -damage + " hitpoints due to an attack by " + attacker.Name + ".") );
                        }
                        else
                        {
                            Context.Current.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + -damage + " hitpoints.") );
                        }
                    }
                }
            }

            if (attacker != target)
            {
                if ( !(attacker is Player) && target is Player player)
                {
                    await Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health + damage) );
                }
            }
        }
    }
}