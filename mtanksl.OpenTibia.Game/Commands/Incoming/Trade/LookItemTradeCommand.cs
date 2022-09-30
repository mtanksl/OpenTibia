using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class LookItemTradeCommand : Command
    {
        public LookItemTradeCommand(Player player, byte windowId, byte index)
        {
            WindowId = windowId;

            Index = index;
        }

        public Player Player { get; set; }

        public byte WindowId { get; set; }

        public byte Index { get; set; }

        public override void Execute(Context context)
        {
            OnComplete(context);
        }
    }
}