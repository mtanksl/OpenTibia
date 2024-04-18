using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class PlayerMoveItemCommand : Command
    {
        public PlayerMoveItemCommand(Player player, Item item, IContainer toContainer, byte toIndex, byte count, bool pathfinding)
        {
            Player = player;

            Item = item;

            ToContainer = toContainer;

            ToIndex = toIndex;

            Count = count;

            Pathfinding = pathfinding;
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public IContainer ToContainer { get; set; }

        public byte ToIndex { get; set; }

        public byte Count { get; set; }

        public bool Pathfinding { get; set; }

        public override Promise Execute()
        {
            return Context.AddCommand(new ItemMoveCommand(Item, ToContainer, ToIndex) );
        }
    }
}