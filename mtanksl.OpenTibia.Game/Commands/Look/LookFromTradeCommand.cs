using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class LookFromTradeCommand : LookCommand
    {
        public LookFromTradeCommand(Player player, byte windowId, byte index) : base(player)
        {
            WindowId = windowId;

            Index = index;
        }

        public byte WindowId { get; set; }

        public byte Index { get; set; }

        public override void Execute(Context context)
        {
            Window window = Player.Client.WindowCollection.GetWindow(WindowId);

            if (window != null)
            {
                Item item = window.GetContent(Index) as Item;

                if (item != null)
                {
                    LookAtItem(item, context);
                }
            }
        }
    }
}