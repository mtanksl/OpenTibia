using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class InventoryAddItemCommand : Command
    {
        private readonly ushort stealthRing;

        public InventoryAddItemCommand(Inventory inventory, byte slot, Item item)
        {
            stealthRing = Context.Server.Values.GetUInt16("values.items.stealthRing");

            Inventory = inventory;

            this.slot = slot;

            Item = item;
        }

        public Inventory Inventory { get; set; }

        public byte slot { get; set; }

        public Item Item { get; set; }

        public override async Promise Execute()
        {
            Inventory.AddContent(Item, slot);

            Context.AddPacket(Inventory.Player, new SlotAddOutgoingPacket(slot, Item ) );

            var maxLightLevel = Inventory.GetItems()
                .Select(i => i.Metadata.Light)
                .Where(l => l != null)
                .OrderByDescending(l => l.Level)
                .FirstOrDefault();

            await Context.AddCommand(new CreatureUpdateLightCommand(Inventory.Player, Inventory.Player.ConditionLight, maxLightLevel) );

            if (Item.Metadata.SlotType == SlotType.Head && (Slot)slot == Slot.Head ||
                Item.Metadata.SlotType == SlotType.Amulet && (Slot)slot == Slot.Amulet ||
                Item.Metadata.SlotType == SlotType.Container && (Slot)slot == Slot.Container ||
                Item.Metadata.SlotType == SlotType.Armor && (Slot)slot == Slot.Armor ||
                (Item.Metadata.SlotType == SlotType.Hand || Item.Metadata.SlotType == SlotType.TwoHand) && ( (Slot)slot == Slot.Left || (Slot)slot == Slot.Right) ||
                Item.Metadata.SlotType == SlotType.Feet && (Slot)slot == Slot.Feet ||
                Item.Metadata.SlotType == SlotType.Ring && (Slot)slot == Slot.Ring ||
                Item.Metadata.SlotType == SlotType.Extra && (Slot)slot == Slot.Extra)
            {
                if (Item.Metadata.OpenTibiaId == stealthRing)
                {
                    await Context.AddCommand(new CreatureUpdateOutfitCommand(Inventory.Player, Inventory.Player.BaseOutfit, Inventory.Player.ConditionOutfit, Inventory.Player.Swimming, Inventory.Player.ConditionStealth, true) );
                }

                if (Item.Metadata.SpeedModifier != null)
                {
                    await Context.AddCommand(new CreatureUpdateSpeedCommand(Inventory.Player, Inventory.Player.ConditionSpeed, Inventory.Player.ItemSpeed + Item.Metadata.SpeedModifier.Value) );
                }

                foreach (var skillModifier in Item.Metadata.SkillModifier)
                {
                    await Context.AddCommand(new PlayerUpdateSkillCommand(Inventory.Player, skillModifier.Key, Inventory.Player.Skills.GetSkillPoints(skillModifier.Key), Inventory.Player.Skills.GetSkillLevel(skillModifier.Key), Inventory.Player.Skills.GetSkillPercent(skillModifier.Key), Inventory.Player.Skills.GetConditionSkillLevel(skillModifier.Key), Inventory.Player.Skills.GetItemSkillLevel(skillModifier.Key) + skillModifier.Value) );
                }
            }

            Context.AddEvent(new InventoryAddItemEventArgs(Inventory, Item, slot) );
        }
    }
}