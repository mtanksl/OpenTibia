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
            new StopWalkCommand(Player).Execute(context);

            new StopAttackCommand(Player).Execute(context);

            new StopFollowCommand(Player).Execute(context);

            base.OnCompleted(context);
        }
    }
}