using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public abstract class UseItemCommand : Command
    {
        public UseItemCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        protected void UseItem(Context context, Item fromItem, byte? containerId)
        {
            context.AddCommand(new PlayerUseItemCommand(Player, fromItem, containerId), ctx =>
            {
                OnComplete(context);
            } );
        }
    }
}