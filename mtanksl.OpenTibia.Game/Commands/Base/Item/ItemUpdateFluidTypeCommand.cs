using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ItemUpdateFluidTypeCommand : Command
    {
        public ItemUpdateFluidTypeCommand(FluidItem item, FluidType fluidType)
        {
            Item = item;

            FluidType = fluidType;
        }

        public FluidItem Item { get; set; }

        public FluidType FluidType { get; set; }

        public override void Execute(Context context)
        {
            if (Item.FluidType != FluidType)
            {
                Item.FluidType = FluidType;

                switch (Item.Container)
                {
                    case Tile tile:
                        {
                            byte index = tile.GetIndex(Item);

                            foreach (var observer in context.Server.GameObjects.GetPlayers() )
                            {
                                if (observer.Tile.Position.CanSee(tile.Position) )
                                {
                                    context.WritePacket(observer.Client.Connection, new ThingUpdateOutgoingPacket(tile.Position, index, Item) );
                                }
                            }
                        }
                        break;

                    case Inventory inventory:
                        {
                            byte slot = inventory.GetIndex(Item);

                            context.WritePacket(inventory.Player.Client.Connection, new SlotAddOutgoingPacket( (Slot)slot, Item) );
                        }
                        break;

                    case Container container:
                        {
                            byte index = container.GetIndex(Item);

                            foreach (var observer in container.GetPlayers() )
                            {
                                foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                                {
                                    if (pair.Value == container)
                                    {
                                        context.WritePacket(observer.Client.Connection, new ContainerUpdateOutgoingPacket(pair.Key, index, Item) );
                                    }
                                }
                            }
                        }
                        break;
                }
            }

            base.OnCompleted(context);
        }
    }
}