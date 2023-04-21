using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureAttackCreatureCommand : Command
    {
        public CreatureAttackCreatureCommand(Creature attacker, Creature target, Attack attack)
        {
            Attacker = attacker;

            Target = target;

            Attack = attack;
        }

        public Creature Attacker { get; set; }

        public Creature Target { get; set; }

        public Attack Attack { get; set; }

        public override Promise Execute()
        {
            int damage = Attack.Calculate(Attacker, Target);

            if (damage == 0)
            {
                if (Target is Player player)
                {
                    if (Attacker == null)
                    {
                        //
                    }
                    else
                    {
                        if (Attacker != Target)
                        {
                            Context.Current.AddPacket(player.Client.Connection, new SetFrameColorOutgoingPacket(Attacker.Id, FrameColor.Black) );
                        }
                    }
                }

                return Attack.Missed(Attacker, Target);
            }

            if (damage < 0)
            {
                if (Target is Player player)
                {
                    if (Attacker == null)
                    {
                        Context.Current.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + -damage + " hitpoints.") );
                    }
                    else
                    {
                        if (Attacker != Target)
                        {
                            Context.Current.AddPacket(player.Client.Connection, new SetFrameColorOutgoingPacket(Attacker.Id, FrameColor.Black) );

                            Context.Current.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + -damage + " hitpoints due to an attack by " + Attacker.Name + ".") );
                        }
                    }
                }

                return Attack.Hit(Attacker, Target, damage);
            }

            return Attack.Hit(Attacker, Target, damage);
        }
    }
}