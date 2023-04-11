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
            return Promise.Run( (resolve, reject) =>
            {
                StringBuilder builder = new StringBuilder();

                builder.Append("You see ");

                if (Item is StackableItem stackableItem && stackableItem.Count > 1 && Item.Metadata.Plural != null)
                {
                    builder.Append(stackableItem.Count + " " + Item.Metadata.Plural);
                }
                else
                {
                    if (Item.Metadata.Article != null)
                    {
                        builder.Append(Item.Metadata.Article + " ");
                    }

                    builder.Append(Item.Metadata.Name);
                }

                if (Item is Container container)
                {
                    builder.Append(" (Vol: " + container.Metadata.Capacity + ")");
                }
                else if (Item is FluidItem fluidItem)
                {
                    switch (fluidItem.FluidType)
                    {
                        case FluidType.Empty:

                            builder.Append(". It is empty");

                            break;

                        case FluidType.Water:

                            builder.Append(" of water");

                            break;

                        case FluidType.Blood:

                            builder.Append(" of blood");

                            break;

                        case FluidType.Beer:

                            builder.Append(" of beer");

                            break;

                        case FluidType.Slime:

                            builder.Append(" of slime");

                            break;

                        case FluidType.Lemonade:

                            builder.Append(" of lemonade");

                            break;

                        case FluidType.Milk:

                            builder.Append(" of milk");

                            break;

                        case FluidType.Manafluid:

                            builder.Append(" of manafluid");

                            break;

                        case FluidType.Lifefluid:

                            builder.Append(" of lifefluid");

                            break;

                        case FluidType.Oil:

                            builder.Append(" of oil");

                            break;

                        case FluidType.Urine:

                            builder.Append(" of urine");

                            break;

                        case FluidType.CoconutMilk:

                            builder.Append(" of coconut milk");

                            break;

                        case FluidType.Wine:

                            builder.Append(" of wine");

                            break;

                        case FluidType.Mud:

                            builder.Append(" of mud");

                            break;

                        case FluidType.FruitJuice:

                            builder.Append(" of fruit juice");

                            break;

                        case FluidType.Lava:

                            builder.Append(" of lava");
                            break;

                        case FluidType.Rum:

                            builder.Append(" of rum");

                            break;
                    }
                }
                else if (Item is SplashItem splashItem)
                {
                    switch (splashItem.FluidType)
                    {
                        case FluidType.Water:

                            builder.Append(" of water");

                            break;

                        case FluidType.Blood:

                            builder.Append(" of blood");

                            break;

                        case FluidType.Beer:

                            builder.Append(" of beer");

                            break;

                        case FluidType.Slime:

                            builder.Append(" of slime");

                            break;

                        case FluidType.Lemonade:

                            builder.Append(" of lemonade");

                            break;

                        case FluidType.Milk:

                            builder.Append(" of milk");

                            break;

                        case FluidType.Manafluid:

                            builder.Append(" of manafluid");

                            break;

                        case FluidType.Lifefluid:

                            builder.Append(" of lifefluid");

                            break;

                        case FluidType.Oil:

                            builder.Append(" of oil");

                            break;

                        case FluidType.Urine:

                            builder.Append(" of urine");

                            break;

                        case FluidType.CoconutMilk:

                            builder.Append(" of coconut milk");

                            break;

                        case FluidType.Wine:

                            builder.Append(" of wine");

                            break;

                        case FluidType.Mud:

                            builder.Append(" of mud");

                            break;

                        case FluidType.FruitJuice:

                            builder.Append(" of fruit juice");

                            break;

                        case FluidType.Lava:

                            builder.Append(" of lava");
                            break;

                        case FluidType.Rum:

                            builder.Append(" of rum");

                            break;
                    }
                }

                builder.Append(". (Id: " + Item.Metadata.OpenTibiaId + ")");

                Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, builder.ToString() ) );

                resolve();
            } );
        }
    }
}