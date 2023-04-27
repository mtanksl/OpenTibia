using OpenTibia.Common.Structures;
using System.Xml.Linq;

namespace OpenTibia.FileFormats.Xml.Items
{
    public class Item
    {
        public static Item Load(XElement itemNode)
        {
            Item item = new Item();

            item.OpenTibiaId = (ushort)(uint)itemNode.Attribute("id");

            item.Article = (string)itemNode.Attribute("article");

            item.Name = (string)itemNode.Attribute("name");

            item.Plural = (string)itemNode.Attribute("plural");

            foreach (var attributeNode in itemNode.Elements("attribute") )
            {
                XAttribute key = attributeNode.Attribute("key");

                XAttribute value = attributeNode.Attribute("value");

                switch ( (string)key )
                {
                    case "article":

                        item.Article = (string)value;

                        break;

                    case "name":

                        item.Name = (string)value;

                        break;

                    case "plural":

                        item.Plural = (string)value;

                        break;

                    case "weight":

                        item.Weight = (uint)value;

                        break;

                    case "floorchange":

                        switch ( (string)value )
	                    {
		                    case "down":

                                item.FloorChange |= FloorChange.Down;

                                break;

                            case "north":

                                item.FloorChange |= FloorChange.North;

                                break;

                            case "south":

                                item.FloorChange |= FloorChange.South;

                                break;

                            case "west":

                                item.FloorChange |= FloorChange.West;

                                break;

                            case "east":

                                item.FloorChange |= FloorChange.East;

                                break;
	                    }

                        break;

                    case "containerSize":

                        item.ContainerSize = (byte)(int)value;

                        break;

                    case "slotType":

                        switch ( (string)value)
                        {
                            case "head":

                                item.SlotType = Slot.Head;

                                break;

                            case "necklace":

                                item.SlotType = Slot.Amulet;

                                break;

                            case "backpack":

                                item.SlotType = Slot.Container;

                                break;

                            case "body":

                                item.SlotType = Slot.Armor;

                                break;

                            case "legs":

                                item.SlotType = Slot.Legs;

                                break;

                            case "feet":

                                item.SlotType = Slot.Feet;

                                break;

                            case "ring":

                                item.SlotType = Slot.Ring;

                                break;

                            case "ammo":

                                item.SlotType = Slot.Extra;

                                break;
                        }

                        break;
                }
            }

            return item;
        }

        public ushort OpenTibiaId { get; set; }

        public string Article { get; set; }

        public string Name { get; set; }

        public string Plural { get; set; }

        public uint Weight { get; set; }

        public FloorChange FloorChange { get; set; }

        public byte ContainerSize { get; set; }

        public Slot? SlotType { get; set; }
    }
}