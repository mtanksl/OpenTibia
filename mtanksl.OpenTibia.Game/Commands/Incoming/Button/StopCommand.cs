using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class StopCommand : Command
    {
        public StopCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override void Execute(Context context)
        {
            context.AddCommand(new StopWalkCommand(Player) );

            context.AddCommand(new StopAttackCommand(Player) );

            context.AddCommand(new StopFollowCommand(Player) );

            base.Execute(context);
        }
    }
}