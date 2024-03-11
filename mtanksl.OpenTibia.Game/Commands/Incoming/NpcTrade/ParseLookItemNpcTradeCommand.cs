using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;
using System.Text;

namespace OpenTibia.Game.Commands
{
    public class ParseLookItemNpcTradeCommand : Command
    {
        public ParseLookItemNpcTradeCommand(Player player, ItemMetadata itemMetadata, byte count)
        {
            Player = player;

            ItemMetadata = itemMetadata;

            Count = count;
        }

        public Player Player { get; set; }

        public ItemMetadata ItemMetadata { get; set; }

        public byte Count { get; set; }

        public override Promise Execute()
        {
            string name;

            if (ItemMetadata.Article != null)
            {
                name = ItemMetadata.Article + " " + ItemMetadata.Name;
            }
            else
            {
                name = ItemMetadata.Name;
            }

            string description = ItemMetadata.Description;

            List<string> attributes = new List<string>();

            if (ItemMetadata.Flags.Is(ItemMetadataFlags.IsContainer) )
            {
                attributes.Add("Vol: " + ItemMetadata.Capacity);
            }
            else if (ItemMetadata.Flags.Is(ItemMetadataFlags.Stackable) )
            {
                if (ItemMetadata.RuneSpellName != null)
                {
                    attributes.Add("\"" + ItemMetadata.RuneSpellName + "\"");

                    attributes.Add("Charges: " + Count);
                }
                else
                {
                    if (Count > 1)
                    {
                        name = Count + " " + (ItemMetadata.Plural ?? ItemMetadata.Name);
                    }
                }
            }
            else if (ItemMetadata.Flags.Is(ItemMetadataFlags.IsFluid) )
            {
                switch ( (FluidType)Count)
                {
                    case FluidType.Empty:

                        description = "It is empty.";

                        break;

                    case FluidType.Water:

                        name += " of water";

                        break;

                    case FluidType.Blood:

                        name += " of blood";

                        break;

                    case FluidType.Beer:

                        name += " of beer";

                        break;

                    case FluidType.Slime:

                        name += " of slime";

                        break;

                    case FluidType.Lemonade:

                        name += " of lemonade";

                        break;

                    case FluidType.Milk:

                        name += " of milk";

                        break;

                    case FluidType.Manafluid:

                        name += " of manafluid";

                        break;

                    case FluidType.Lifefluid:

                        name += " of lifefluid";

                        break;

                    case FluidType.Oil:

                        name += " of oil";

                        break;

                    case FluidType.Urine:

                        name += " of urine";

                        break;

                    case FluidType.CoconutMilk:

                        name += " of coconut milk";

                        break;

                    case FluidType.Wine:

                        name += " of wine";

                        break;

                    case FluidType.Mud:

                        name += " of mud";

                        break;

                    case FluidType.FruitJuice:

                        name += " of fruit juice";

                        break;

                    case FluidType.Lava:

                        name += " of lava";

                        break;

                    case FluidType.Rum:

                        name += " of rum";

                        break;
                }
            }
            else if (ItemMetadata.Flags.Is(ItemMetadataFlags.IsSplash) )
            {
                switch ( (FluidType)Count)
                {
                    case FluidType.Empty:

                        description = "It is empty.";

                        break;

                    case FluidType.Water:

                        name += " of water";

                        break;

                    case FluidType.Blood:

                        name += " of blood";

                        break;

                    case FluidType.Beer:

                        name += " of beer";

                        break;

                    case FluidType.Slime:

                        name += " of slime";

                        break;

                    case FluidType.Lemonade:

                        name += " of lemonade";

                        break;

                    case FluidType.Milk:

                        name += " of milk";

                        break;

                    case FluidType.Manafluid:

                        name += " of manafluid";

                        break;

                    case FluidType.Lifefluid:

                        name += " of lifefluid";

                        break;

                    case FluidType.Oil:

                        name += " of oil";

                        break;

                    case FluidType.Urine:

                        name += " of urine";

                        break;

                    case FluidType.CoconutMilk:

                        name += " of coconut milk";

                        break;

                    case FluidType.Wine:

                        name += " of wine";

                        break;

                    case FluidType.Mud:

                        name += " of mud";

                        break;

                    case FluidType.FruitJuice:

                        name += " of fruit juice";

                        break;

                    case FluidType.Lava:

                        name += " of lava";

                        break;

                    case FluidType.Rum:

                        name += " of rum";

                        break;
                }
            }
                       
            if (ItemMetadata.Armor != null)
            {
                attributes.Add("Arm: " + ItemMetadata.Armor);
            }

            if (ItemMetadata.Range != null)
            {
                attributes.Add("Range: " + ItemMetadata.Range);
            }

            if (ItemMetadata.Attack != null)
            {
                attributes.Add("Atk: " + ItemMetadata.Attack);
            }

            if (ItemMetadata.Defense != null)
            {
                attributes.Add("Def: " + ItemMetadata.Defense);
            }

            if (Player.Rank == Rank.Gamemaster)
            {
                attributes.Add("Item Id: " + ItemMetadata.OpenTibiaId);
            }

            StringBuilder builder = new StringBuilder();

            builder.Append("You see " + name);

            if (attributes.Count > 0)
            {
                builder.Append(" (" + string.Join(", ", attributes) + ")");
            }

            builder.Append(".");

            if (description != null)
            {
                builder.Append(" " + description);
            }

            Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, builder.ToString() ) );

            return Promise.Completed;
        }
    }
}