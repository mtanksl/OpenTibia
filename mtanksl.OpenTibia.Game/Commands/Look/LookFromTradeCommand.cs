using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class LookFromTradeCommand : Command
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
                    LookItemCommand command = new LookItemCommand(Player, item);

                    command.Completed += (s, e) =>
                    {
                        //Act

                        base.Execute(server, context);
                    };

                    command.Execute(server, context);
                }
            }
        }
    }
}