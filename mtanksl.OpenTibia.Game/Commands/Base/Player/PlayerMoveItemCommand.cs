using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class PlayerMoveItemCommand : Command
    {
        public PlayerMoveItemCommand(Player player, Item item, IContainer toContainer, byte toIndex, byte count)
        {
            Player = player;

            Item = item;

            ToContainer = toContainer;

            ToIndex = toIndex;

            Count = count;
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public IContainer ToContainer { get; set; }

        public byte ToIndex { get; set; }

        public byte Count { get; set; }

        public override void Execute(Context context)
        {
            List<Command> commands = new List<Command>();

            switch (Item.Container)
            {
                case Tile fromTile:

                    commands.Add(new TileRemoveItemCommand(fromTile, Item) );

                    break;

                case Inventory fromInventory:

                    commands.Add(new InventoryRemoveItemCommand(fromInventory, Item) );

                    break;

                case Container fromContainer:

                    commands.Add(new ContainerRemoveItemCommand(fromContainer, Item) );

                    break;
            }

            switch (ToContainer)
            {
                case Tile toTile:

                    commands.Add(new TileAddItemCommand(toTile, Item) );

                    break;

                case Inventory toInventory:

                    commands.Add(new InventoryAddItemCommand(toInventory, ToIndex, Item) );

                    break;

                case Container toContainer:

                    commands.Add(new ContainerAddItemCommand(toContainer, Item) );

                    break;               
            }

            Command command = new SequenceCommand(commands.ToArray() );

            command.Completed += (s, e) =>
            {
                base.OnCompleted(e.Context);
            };

            command.Execute(context);
        }
    }
}