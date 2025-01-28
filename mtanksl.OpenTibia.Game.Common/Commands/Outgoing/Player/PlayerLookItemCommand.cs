using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OpenTibia.Game.Commands
{
    public class PlayerLookItemCommand : Command
    {
        public PlayerLookItemCommand(Player player, Item item)
        {
            Player = player;

            Item = item;

            ItemMetadata = item.Metadata;

            if (item is StackableItem stackableItem)
            {
                Type = stackableItem.Count;
            }
            else if (item is FluidItem fluidItem)
            {
                Type = (byte)fluidItem.FluidType;
            }
            else if (item is SplashItem splashItem)
            {
                Type = (byte)splashItem.FluidType;
            }
            else
            {
                Type = 1;
            }
        }

        public PlayerLookItemCommand(Player player, ItemMetadata itemMetadata, byte type)
        {
            Player = player;

            ItemMetadata = itemMetadata;

            Type = type;
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public ItemMetadata ItemMetadata { get; set; }

        public byte Type { get; set; }

        public override Promise Execute()
        {
            string decription = ItemMetadata.GetDescription(Type);

            List<string> attributes = new List<string>();

            if (ItemMetadata.RuneSpellName != null)
            {
                attributes.Add("\"" + ItemMetadata.RuneSpellName + "\"");

                attributes.Add("Charges: " + Type);
            }

            if (ItemMetadata.Flags.Is(ItemMetadataFlags.IsContainer) )
            {
                attributes.Add("Vol: " + ItemMetadata.Capacity);
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
                if (ItemMetadata.ExtraDefense != null)
                {
                    attributes.Add("Def: " + ItemMetadata.Defense + " +" + ItemMetadata.ExtraDefense);
                }
                else
                {
                    attributes.Add("Def: " + ItemMetadata.Defense);
                }
            }

            if (Player.Rank == Rank.Gamemaster)
            {
                attributes.Add("Item Id: " + ItemMetadata.OpenTibiaId);

                if (Item != null)
                {
                    if (Item.ActionId > 0)
                    {
                        attributes.Add("Action Id: " + Item.ActionId);
                    }

                    if (Item.UniqueId > 0)
                    {
                        attributes.Add("Unique Id: " + Item.UniqueId);
                    }
                }
            }

            List<string> descriptions = new List<string>();
            
            if (ItemMetadata.Flags.Is(ItemMetadataFlags.IsFluid) || ItemMetadata.Flags.Is(ItemMetadataFlags.IsSplash) )
            {
                switch ( (FluidType)Type)
                {
                    case FluidType.Empty:

                        descriptions.Add("It is empty.");

                        break;

                    case FluidType.Water:

                        decription += " of water";

                        break;

                    case FluidType.Blood:

                        decription += " of blood";

                        break;

                    case FluidType.Beer:

                        decription += " of beer";

                        break;

                    case FluidType.Slime:

                        decription += " of slime";

                        break;

                    case FluidType.Lemonade:

                        decription += " of lemonade";

                        break;

                    case FluidType.Milk:

                        decription += " of milk";

                        break;

                    case FluidType.Manafluid:

                        decription += " of manafluid";

                        break;

                    case FluidType.Lifefluid:

                        decription += " of lifefluid";

                        break;

                    case FluidType.Oil:

                        decription += " of oil";

                        break;

                    case FluidType.Urine:

                        decription += " of urine";

                        break;

                    case FluidType.CoconutMilk:

                        decription += " of coconut milk";

                        break;

                    case FluidType.Wine:

                        decription += " of wine";

                        break;

                    case FluidType.Mud:

                        decription += " of mud";

                        break;

                    case FluidType.FruitJuice:

                        decription += " of fruit juice";

                        break;

                    case FluidType.Lava:

                        decription += " of lava";

                        break;

                    case FluidType.Rum:

                        decription += " of rum";

                        break;
                }
            }

            if (Item != null)
            {
                if (ItemMetadata.Flags.Is(ItemMetadataFlags.Pickupable) && Item.UniqueId == 0)
                {
                    uint weight = Item.GetWeight();

                    if (weight > 0)
                    {
                        if ( (Item.Parent is Tile tile && Player.Tile.Position.IsInRange(tile.Position, 1) ) || Item.Parent is Inventory || Item.Parent is Container)
                        {
                            descriptions.Add("It weights " + (weight / 100.0).ToString("0.00", CultureInfo.InvariantCulture) + " oz.");
                        }
                    }
                }

                if (ItemMetadata.Flags.Is(ItemMetadataFlags.AllowDistanceRead) && Item is ReadableItem readableItem)
                {
                    if (Player.Tile.Position.IsInRange( ( (Tile)Item.Parent).Position, 4) )
                    {
                        if (readableItem.Text != null)
                        {
                            descriptions.Add("You read: " + readableItem.Text + ".");
                        }
                        else
                        {
                            descriptions.Add("Nothing is written on it.");
                        }
                    }
                    else
                    {
                        descriptions.Add("You are too far away to read it.");
                    } 
                }

                if (Item.Parent is HouseTile houseTile && Item is DoorItem doorItem)
                {
                    if (houseTile.House.Owner != null)
                    {
                        descriptions.Add("It belongs to house '" + houseTile.House.Name + "'. " + houseTile.House.Owner + " owns this house.");
                    }
                    else
                    {
                        descriptions.Add("It belongs to house '" + houseTile.House.Name + "'. Nobody owns this house. It costs " + (houseTile.House.Rent * 5) + " gold coins.");
                    }
                }
            }

            if (ItemMetadata.Description != null)
            {
                descriptions.Add(ItemMetadata.Description);
            }

            StringBuilder builder = new StringBuilder();

            builder.Append("You see " + decription);

            if (attributes.Count > 0)
            {
                builder.Append(" (" + string.Join(", ", attributes) + ")");
            }

            builder.Append(".");

            if (descriptions.Count > 0)
            {
                builder.Append(" " + string.Join(" ", descriptions) );
            }

            Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, builder.ToString() ) );

            return Promise.Completed;
        }
    }
}