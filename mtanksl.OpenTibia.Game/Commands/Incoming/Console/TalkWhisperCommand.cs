using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class TalkWhisperCommand : Command
    {
        public TalkWhisperCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override void Execute(Context context)
        {
            context.AddCommand(new PlayerWhisperCommand(Player, Message), ctx =>
            {
                base.Execute(ctx);
            } );
        }
    }
}