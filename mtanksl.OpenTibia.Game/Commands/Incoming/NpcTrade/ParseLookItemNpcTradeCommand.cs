using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;
using System.Text;

namespace OpenTibia.Game.Commands
{
    public class ParseLookItemNpcTradeCommand : Command
    {
        public ParseLookItemNpcTradeCommand(Player player, ushort tibiaId, byte count)
        {
            Player = player;

            TibiaId = tibiaId;

            Count = count;
        }

        public Player Player { get; set; }

        public ushort TibiaId { get; set; }

        public byte Count { get; set; }

        public override Promise Execute()
        {
            if (Context.Server.Config.GamePrivateNpcSystem)
            {
                ItemMetadata itemMetadata = Context.Server.ItemFactory.GetItemMetadataByTibiaId(TibiaId);

                string name;

                if (itemMetadata.Article != null)
                {
                    name = itemMetadata.Article + " " + itemMetadata.Name;
                }
                else
                {
                    name = itemMetadata.Name;
                }

                string description = itemMetadata.Description;

                List<string> attributes = new List<string>();

                if (itemMetadata.Flags.Is(ItemMetadataFlags.IsContainer) )
                {
                    attributes.Add("Vol: " + itemMetadata.Capacity);
                }
                else if (itemMetadata.Flags.Is(ItemMetadataFlags.Stackable) )
                {
                    if (itemMetadata.RuneSpellName != null)
                    {
                        attributes.Add("\"" + itemMetadata.RuneSpellName + "\"");

                        attributes.Add("Charges: " + Count);
                    }
                    else
                    {
                        if (Count > 1)
                        {
                            name = Count + " " + (itemMetadata.Plural ?? itemMetadata.Name);
                        }
                    }
                }
                else if (itemMetadata.Flags.Is(ItemMetadataFlags.IsFluid) )
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
                else if (itemMetadata.Flags.Is(ItemMetadataFlags.IsSplash) )
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
                       
                if (itemMetadata.Armor != null)
                {
                    attributes.Add("Arm: " + itemMetadata.Armor);
                }

                if (itemMetadata.Range != null)
                {
                    attributes.Add("Range: " + itemMetadata.Range);
                }

                if (itemMetadata.Attack != null)
                {
                    attributes.Add("Atk: " + itemMetadata.Attack);
                }

                if (itemMetadata.Defense != null)
                {
                    attributes.Add("Def: " + itemMetadata.Defense);
                }

                if (Player.Rank == Rank.Gamemaster)
                {
                    attributes.Add("Item Id: " + itemMetadata.OpenTibiaId);
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
            }

            return Promise.Completed;
        }
    }
}