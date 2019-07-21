using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class LookFromTradeCommand : LookCommand
    {
        public LookFromTradeCommand(Player player, byte windowId, byte index)
        {
            Player = player;

            WindowId = windowId;

            Index = index;
        }

        public Player Player { get; set; }

        public byte WindowId { get; set; }

        public byte Index { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Window window = Player.Client.WindowCollection.GetWindow(WindowId);

            if (window != null)
            {
                Item item = window.GetContent(Index) as Item;

                if (item != null)
                {
                    //Act

                    Look(Player, item, server, context);

                    base.Execute(server, context);
                }
            }
        }
    }
}