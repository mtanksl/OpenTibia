using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class CloseNpcTradeCommand : Command
    {
        public CloseNpcTradeCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            //Act

            //Notify

            base.Execute(context);
        }
    }
}