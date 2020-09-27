using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class SayCommand : Command
    {
        public SayCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override void Execute(Context context)
        {
            Command command = context.TransformCommand(new PlayerSayCommand(Player, Message) );

            command.Completed += (s, e) =>
            {
                base.OnCompleted(e.Context);
            };

            command.Execute(context);
        }
    }
}