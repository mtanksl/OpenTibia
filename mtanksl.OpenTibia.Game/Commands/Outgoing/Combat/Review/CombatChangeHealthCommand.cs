using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CombatChangeHealthCommand : Command
    {
        public CombatChangeHealthCommand(Creature attacker, Creature target, AnimatedTextColor? animatedTextColor, int health)
        {
            Attacker = attacker;

            Target = target;

            AnimatedTextColor = animatedTextColor;

            Health = health;
        }

        public Creature Attacker { get; set; }

        public Creature Target { get; set; }

        public AnimatedTextColor? AnimatedTextColor { get; set; }

        public int Health { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                if (Health < 0)
                {
                    if (Target is Player player)
                    {
                        if (Attacker == null)
                        {
                            Context.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + (-Health) + " hitpoints.") );
                        }
                        else if (Attacker == Target)
                        {
                            Context.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + (-Health) + " hitpoints due to your own attack.") );
                        }
                        else
                        {
                            Context.AddPacket(player.Client.Connection, new SetFrameColorOutgoingPacket(Attacker.Id, FrameColor.Black) );

                            Context.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + (-Health) + " hitpoints due to an attack by " + Attacker.Name + ".") );
                        }

                        if (AnimatedTextColor != null)
                        {
                            Context.AddCommand(new ShowAnimatedTextCommand(Target.Tile.Position, AnimatedTextColor.Value, (-Health).ToString() ) );
                        }

                        if (Target.Health + Health > 0)
                        {
                            Context.AddCommand(new CreatureUpdateHealthCommand(Target, (ushort)(Target.Health + Health), Target.MaxHealth) );
                        }
                        else
                        {
                            Context.AddCommand(new CreatureUpdateHealthCommand(Target, 0, Target.MaxHealth) );

                            Context.AddCommand(new TileCreateItemCommand(Target.Tile, 3058, 1) ).Then( (item) => 
                            { 
                                return Context.AddCommand(new ItemDecayTransformCommand(item, 10000, 3059, 1) );

                            } ).Then( (item) => 
                            {
                                return Context.AddCommand(new ItemDecayTransformCommand(item, 10000, 3060, 1) );

                            } ).Then( (item) => 
                            {
                                return Context.AddCommand(new ItemDecayDestroyCommand(item, 10000) );
                            } );

                            Context.AddCommand(new PlayerDestroyCommand(player) );

                            Context.AddPacket(player.Client.Connection, new OpenYouAreDeathDialogOutgoingPacket() );
                        }
                    }
                    else if (Target is Monster monster)
                    {
                        if (AnimatedTextColor != null)
                        {
                            Context.AddCommand(new ShowAnimatedTextCommand(Target.Tile.Position, AnimatedTextColor.Value, (-Health).ToString() ) );
                        }

                        if (Target.Health + Health > 0)
                        {
                            Context.AddCommand(new CreatureUpdateHealthCommand(Target, (ushort)(Target.Health + Health), Target.MaxHealth) );
                        }
                        else
                        {
                            Context.AddCommand(new CreatureUpdateHealthCommand(Target, 0, Target.MaxHealth) );

                            Context.AddCommand(new TileCreateItemCommand(Target.Tile, monster.Metadata.Corpse, 1) ).Then( (item) => 
                            {
                                return Context.AddCommand(new ItemDecayDestroyCommand(item, 10000) );
                            } );

                            Context.AddCommand(new MonsterDestroyCommand(monster) );
                        }
                    }
                }
                else
                {
                    if (Target.Health + Health < Target.MaxHealth)
                    {
                        Context.AddCommand(new CreatureUpdateHealthCommand(Target, (ushort)(Target.Health + Health), Target.MaxHealth) );
                    }
                    else
                    {
                        Context.AddCommand(new CreatureUpdateHealthCommand(Target, Target.MaxHealth, Target.MaxHealth) );
                    }
                }

                resolve();
            } );
        }
    }
}