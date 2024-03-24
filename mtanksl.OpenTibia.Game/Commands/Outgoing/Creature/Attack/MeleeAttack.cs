using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class MeleeAttack : Attack
    {
       public MeleeAttack(int min, int max)
        {
            Min = min;

            Max = max;
        }

        public int Min { get; set; }

        public int Max { get; set; }

        public override int Calculate(Creature attacker, Creature target)
        {
            return -Context.Current.Server.Randomization.Take(Min, Max);
        }

        public override async Promise Missed(Creature attacker, Creature target)
        {
            await Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, MagicEffectType.Puff) );

            if ( !(attacker is Player && (target is Player || target is Npc) ) )
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
            await Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, MagicEffectType.RedSpark) );

            if ( !(attacker is Player && (target is Player || target is Npc) ) )
            {
                await Context.Current.AddCommand(new ShowAnimatedTextCommand(target.Tile.Position, AnimatedTextColor.DarkRed, (-damage).ToString() ) );

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