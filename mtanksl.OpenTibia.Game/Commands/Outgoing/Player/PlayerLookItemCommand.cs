using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Text;

namespace OpenTibia.Game.Commands
{
    public class PlayerLookItemCommand : Command
    {
        public PlayerLookItemCommand(Player player, Item item)
        {
            Player = player;

            Item = item;
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public override Promise Execute()
        {
            StringBuilder builder = new StringBuilder();

            if (Item is StackableItem stackableItem && stackableItem.Count > 1)
            {
                builder.Append("You see " + stackableItem.Count + " " + (Item.Metadata.Plural != null ? Item.Metadata.Plural : Item.Metadata.Name) );
            }
            else
            {
                builder.Append("You see " + (Item.Metadata.Article != null ? Item.Metadata.Article + " " : null) + Item.Metadata.Name);
            }

            switch (Item)
            {
                case Container container:

                    builder.Append(" (Vol: " + container.Metadata.Capacity + ").");

                    break;

                case FluidItem fluidItem:

                    switch (fluidItem.FluidType)
                    {
                        case FluidType.Empty:

                            builder.Append(". It is empty.");

                            break;

                        case FluidType.Water:

                            builder.Append(" of water.");

                            break;

                        case FluidType.Blood:

                            builder.Append(" of blood.");

                            break;

                        case FluidType.Beer:

                            builder.Append(" of beer.");

                            break;

                        case FluidType.Slime:

                            builder.Append(" of slime.");

                            break;

                        case FluidType.Lemonade:

                            builder.Append(" of lemonade.");

                            break;

                        case FluidType.Milk:

                            builder.Append(" of milk.");

                            break;

                        case FluidType.Manafluid:

                            builder.Append(" of manafluid.");

                            break;

                        case FluidType.Lifefluid:

                            builder.Append(" of lifefluid.");

                            break;

                        case FluidType.Oil:

                            builder.Append(" of oil.");

                            break;

                        case FluidType.Urine:

                            builder.Append(" of urine.");

                            break;

                        case FluidType.CoconutMilk:

                            builder.Append(" of coconut milk.");

                            break;

                        case FluidType.Wine:

                            builder.Append(" of wine.");

                            break;

                        case FluidType.Mud:

                            builder.Append(" of mud.");

                            break;

                        case FluidType.FruitJuice:

                            builder.Append(" of fruit juice.");

                            break;

                        case FluidType.Lava:

                            builder.Append(" of lava.");
                            break;

                        case FluidType.Rum:

                            builder.Append(" of rum.");

                            break;
                    }

                    break;

                case SplashItem splashItem:

                    switch (splashItem.FluidType)
                    {
                        case FluidType.Water:

                            builder.Append(" of water.");

                            break;

                        case FluidType.Blood:

                            builder.Append(" of blood.");

                            break;

                        case FluidType.Beer:

                            builder.Append(" of beer.");

                            break;

                        case FluidType.Slime:

                            builder.Append(" of slime.");

                            break;

                        case FluidType.Lemonade:

                            builder.Append(" of lemonade.");

                            break;

                        case FluidType.Milk:

                            builder.Append(" of milk.");

                            break;

                        case FluidType.Manafluid:

                            builder.Append(" of manafluid.");

                            break;

                        case FluidType.Lifefluid:

                            builder.Append(" of lifefluid.");

                            break;

                        case FluidType.Oil:

                            builder.Append(" of oil.");

                            break;

                        case FluidType.Urine:

                            builder.Append(" of urine.");

                            break;

                        case FluidType.CoconutMilk:

                            builder.Append(" of coconut milk.");

                            break;

                        case FluidType.Wine:

                            builder.Append(" of wine.");

                            break;

                        case FluidType.Mud:

                            builder.Append(" of mud.");

                            break;

                        case FluidType.FruitJuice:

                            builder.Append(" of fruit juice.");

                            break;

                        case FluidType.Lava:

                            builder.Append(" of lava.");
                            break;

                        case FluidType.Rum:

                            builder.Append(" of rum.");

                            break;
                    }

                    break;

                default:

                    builder.Append(".");

                    break;
            }

            if (Player.Vocation == Vocation.Gamemaster)
            {
                builder.Remove(builder.Length - 1, 1);

                builder.Append(" (Item Id: " + Item.Metadata.OpenTibiaId + ").");
            }

            Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, builder.ToString() ) );

            return Promise.Completed;
        }
    }
}