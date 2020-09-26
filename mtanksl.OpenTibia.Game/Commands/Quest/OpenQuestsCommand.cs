using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class OpenQuestsCommand : Command
    {
        public OpenQuestsCommand(Player player)
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