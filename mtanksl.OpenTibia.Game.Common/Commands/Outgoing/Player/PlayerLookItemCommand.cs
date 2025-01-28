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
            StringBuilder builder = new StringBuilder();

            builder.Append("You see " + ItemMetadata.GetDescription(Type) );

            List<string> attributes = new List<string>();

            if (ItemMetadata.RuneSpellName != null)
            {
                attributes.Add("\"" + ItemMetadata.RuneSpellName + "\"");
            }

            if (ItemMetadata.Flags.Is(ItemMetadataFlags.IsContainer) )
            {
                attributes.Add("Vol: " + ItemMetadata.Capacity);
            }

            if (ItemMetadata.Attack != null)
            {
                attributes.Add("Atk: " + ItemMetadata.Attack);
            }

            if (ItemMetadata.Range != null)
            {
                attributes.Add("Range: " + ItemMetadata.Range);
            }

            if (ItemMetadata.Defense != null)
            {
                if (ItemMetadata.ExtraDefense != null)
                {
                    attributes.Add("Def: " + ItemMetadata.Defense + " " + (ItemMetadata.ExtraDefense > 0 ? "+" + ItemMetadata.ExtraDefense : ItemMetadata.ExtraDefense) );
                }
                else
                {
                    attributes.Add("Def: " + ItemMetadata.Defense);
                }
            }

            if (ItemMetadata.Armor != null)
            {
                attributes.Add("Arm: " + ItemMetadata.Armor);
            }

            if (ItemMetadata.DamageTakenFromElements.Count > 0)
            {
                foreach (var item in ItemMetadata.DamageTakenFromElements)
                {
                    attributes.Add(item.Key.GetDescription() + " " + (item.Value > 0 ? "+" + item.Value * 100 : item.Value * 100) + "%");
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

            if (attributes.Count > 0)
            {
                builder.Append(" (" + string.Join(", ", attributes) + ")");
            }

            builder.Append(".");

            List<string> descriptions = new List<string>();

            if (ItemMetadata.Description != null)
            {
                descriptions.Add(ItemMetadata.Description);
            }

            if (Item != null)
            {
                if (ItemMetadata.Flags.Is(ItemMetadataFlags.Pickupable) && Item.UniqueId == 0)
                {
                    uint weight = Item.GetWeight();

                    if (weight > 0 && ( (Item.Parent is Tile tile && Player.Tile.Position.IsInRange(tile.Position, 1) ) || Item.Parent is Inventory || Item.Parent is Container) )
                    {
                        descriptions.Add("It weights " + (weight / 100.0).ToString("0.00", CultureInfo.InvariantCulture) + " oz.");
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
                        
            if (descriptions.Count > 0)
            {
                builder.Append(" " + string.Join(" ", descriptions) );
            }

            Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, builder.ToString() ) );

            return Promise.Completed;
        }
    }
}