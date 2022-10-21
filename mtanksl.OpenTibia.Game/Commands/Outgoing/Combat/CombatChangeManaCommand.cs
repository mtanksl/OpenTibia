using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CombatChangeManaCommand : Command
    {
        public CombatChangeManaCommand(Creature attacker, Player target, AnimatedTextColor? animatedTextColor, int mana)
        {
            Attacker = attacker;

            Target = target;

            AnimatedTextColor = animatedTextColor;

            Mana = mana;
        }

        public Creature Attacker { get; set; }

        public Player Target { get; set; }

        public AnimatedTextColor? AnimatedTextColor { get; set; }

        public int Mana { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                if (Mana < 0)
                {
                    if (Attacker == null)
                    {
                        context.AddPacket(Target.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + (-Mana) + " mana.") );
                    }
                    else if (Attacker == Target)
                    {
                        context.AddPacket(Target.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + (-Mana) + " mana due to your own attack.") );
                    }
                    else
                    {
                        context.AddPacket(Target.Client.Connection, new SetFrameColorOutgoingPacket(Attacker.Id, FrameColor.Black) );

                        context.AddPacket(Target.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + (-Mana) + " mana due to an attack by " + Attacker.Name + ".") );
                    }

                    if (AnimatedTextColor != null)
                    {
                        context.AddCommand(new ShowAnimatedTextCommand(Target.Tile.Position, AnimatedTextColor.Value, (-Mana).ToString() ) );
                    }

                    if (Target.Mana + Mana > 0)
                    {
                        context.AddCommand(new PlayerUpdateManaCommand(Target, (ushort)(Target.Mana + Mana), Target.MaxMana) );
                    }
                    else
                    {
                        context.AddCommand(new PlayerUpdateManaCommand(Target, 0, Target.MaxMana) );

                    }
                }
                else
                {
                    if (Target.Mana + Mana < Target.MaxMana)
                    {
                        context.AddCommand(new PlayerUpdateManaCommand(Target, (ushort)(Target.Mana + Mana), Target.MaxMana) );
                    }
                    else
                    {
                        context.AddCommand(new PlayerUpdateManaCommand(Target, Target.MaxMana, Target.MaxMana) );
                    }
                }

                resolve(context);
            } );
        }
    }
}