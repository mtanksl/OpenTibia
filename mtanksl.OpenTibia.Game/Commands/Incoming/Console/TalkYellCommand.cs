using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class TalkYellCommand : Command
    {
        public TalkYellCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override void Execute(Context context)
        {
            context.AddCommand(new PlayerYellCommand(Player, Message) ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}