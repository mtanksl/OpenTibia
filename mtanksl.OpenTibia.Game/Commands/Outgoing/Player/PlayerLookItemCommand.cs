using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
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
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public override Promise Execute()
        {
            string name;

            if (Item.Metadata.Article != null)
            {
                name = Item.Metadata.Article + " " + Item.Metadata.Name;
            }
            else
            {
                name = Item.Metadata.Name;
            }

            List<string> descriptions = new List<string>();

            uint weight = Item.Weight;

            if (weight > 0)
            {
                IContainer root = Item.Root();

                if ( (root is Tile tile && Player.Tile.Position.IsInRange(tile.Position, 1) ) || root is Inventory || root is Safe)
                {
                    descriptions.Add("It weights " + (weight / 100.0).ToString("0.00", CultureInfo.InvariantCulture) + " oz.");
                }
            }

            if (Item.Metadata.Description != null)
            {
                descriptions.Add(Item.Metadata.Description);
            }

            List<string> attributes = new List<string>();

            if (Item is Container container)
            {
                attributes.Add("Vol: " + container.Metadata.Capacity);
            }
            else if (Item is StackableItem stackableItem)
            {
                if (Item.Metadata.RuneSpellName != null)
                {
                    attributes.Add("\"" + Item.Metadata.RuneSpellName + "\"");

                    attributes.Add("Charges: " + stackableItem.Count);
                }
                else
                {
                    if (stackableItem.Count > 1)
                    {
                        name = stackableItem.Count + " " + (Item.Metadata.Plural ?? Item.Metadata.Name);
                    }
                }
            }
            else if (Item is FluidItem fluidItem)
            {
                switch (fluidItem.FluidType)
                {
                    case FluidType.Empty:

                        descriptions.Add("It is empty.");

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
            else if (Item is SplashItem splashItem)
            {
                switch (splashItem.FluidType)
                {
                    case FluidType.Empty:

                        descriptions.Add("It is empty.");

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
            else if (Item is SignItem signItem)
            {                
                if (Player.Tile.Position.IsInRange( ( (Tile)Item.Parent).Position, 4) )
                {
                    if (signItem.Text != null)
                    {
                        descriptions.Add("You read: " + signItem.Text + ".");
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
                       
            if (Item.Metadata.Armor != null)
            {
                attributes.Add("Arm: " + Item.Metadata.Armor);
            }

            if (Item.Metadata.Range != null)
            {
                attributes.Add("Range: " + Item.Metadata.Range);
            }

            if (Item.Metadata.Attack != null)
            {
                attributes.Add("Atk: " + Item.Metadata.Attack);
            }

            if (Item.Metadata.Defense != null)
            {
                attributes.Add("Def: " + Item.Metadata.Defense);
            }

            if (Player.Rank == Rank.Gamemaster)
            {
                attributes.Add("Item Id: " + Item.Metadata.OpenTibiaId);
            }

            StringBuilder builder = new StringBuilder();

            builder.Append("You see " + name);

            if (attributes.Count > 0)
            {
                builder.Append(" (" + string.Join(", ", attributes) + ")");
            }

            builder.Append(".");

            if (descriptions.Count > 0)
            {
                builder.Append(" " + string.Join(" ", descriptions) );
            }

            Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, builder.ToString() ) );

            return Promise.Completed;
        }
    }
}