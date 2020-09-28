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

        protected void UseItem(Item fromItem, byte? containerId, Context context)
        {
            Command command = context.TransformCommand(new PlayerUseItemCommand(Player, fromItem, containerId) );

            command.Completed += (s, e) =>
            {
                base.OnCompleted(e.Context);
            };

            command.Execute(context);
        }
    }
}