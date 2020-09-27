using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class TurnCommand : Command
    {
        public TurnCommand(Player player, Direction direction)
        {
            Player = player;

            Direction = direction;
        }

        public Player Player { get; set; }

        public Direction Direction { get; set; }

        public override void Execute(Context context)
        {
            Command command = context.TransformCommand(new CreatureUpdateDirectionCommand(Player, Direction) );

            command.Completed += (s, e) =>
            {
                base.OnCompleted(e.Context);
            };

            command.Execute(context);
        }
    }
}