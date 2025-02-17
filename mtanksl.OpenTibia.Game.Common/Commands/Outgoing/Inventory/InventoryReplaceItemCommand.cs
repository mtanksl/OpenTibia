using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class InventoryReplaceItemCommand : Command
    {
        private readonly ushort stealthRing;

        public InventoryReplaceItemCommand(Inventory inventory, Item fromItem, Item toItem)
        {
            stealthRing = Context.Server.Values.GetUInt16("values.items.stealthRing");

            Inventory = inventory;

            FromItem = fromItem;

            ToItem = toItem;
        }

        public Inventory Inventory { get; set; }

        public Item FromItem { get; set; }

        public Item ToItem { get; set; }

        public override async Promise Execute()
        {
            byte slot = (byte)Inventory.GetIndex(FromItem);

            Inventory.ReplaceContent(slot, ToItem);

            Context.AddPacket(Inventory.Player, new SlotAddOutgoingPacket(slot, ToItem ) );

            var maxLightLevel = Inventory.GetItems()
                .Select(i => i.Metadata.Light)
                .Where(l => l != null)
                .OrderByDescending(l => l.Level)
                .FirstOrDefault();

            await Context.AddCommand(new CreatureUpdateLightCommand(Inventory.Player, Inventory.Player.ConditionLight, maxLightLevel) );

            if (FromItem.Metadata.SlotType == SlotType.Head && (Slot)slot == Slot.Head ||
                FromItem.Metadata.SlotType == SlotType.Necklace && (Slot)slot == Slot.Necklace ||
                FromItem.Metadata.SlotType == SlotType.Backpack && (Slot)slot == Slot.Backpack ||
                FromItem.Metadata.SlotType == SlotType.Body && (Slot)slot == Slot.Body ||
                (FromItem.Metadata.SlotType == SlotType.Hand || FromItem.Metadata.SlotType == SlotType.TwoHanded) && ( (Slot)slot == Slot.Left || (Slot)slot == Slot.Right) ||
                FromItem.Metadata.SlotType == SlotType.Feet && (Slot)slot == Slot.Feet ||
                FromItem.Metadata.SlotType == SlotType.Ring && (Slot)slot == Slot.Ring ||
                FromItem.Metadata.SlotType == SlotType.Ammo && (Slot)slot == Slot.Ammo)
            {
                if (FromItem.Metadata.OpenTibiaId == stealthRing)
                {
                    await Context.AddCommand(new CreatureUpdateOutfitCommand(Inventory.Player, Inventory.Player.BaseOutfit, Inventory.Player.ConditionOutfit, Inventory.Player.Swimming, Inventory.Player.ConditionStealth, false) );
                }

                if (FromItem.Metadata.SpeedModifier != null)
                {
                    await Context.AddCommand(new CreatureUpdateSpeedCommand(Inventory.Player, Inventory.Player.ConditionSpeed, Inventory.Player.ItemSpeed - FromItem.Metadata.SpeedModifier.Value) );
                }

                foreach (var skillModifier in FromItem.Metadata.SkillModifier)
                {
                    await Context.AddCommand(new PlayerUpdateSkillCommand(Inventory.Player, skillModifier.Key, Inventory.Player.Skills.GetSkillPoints(skillModifier.Key), Inventory.Player.Skills.GetSkillLevel(skillModifier.Key), Inventory.Player.Skills.GetSkillPercent(skillModifier.Key), Inventory.Player.Skills.GetConditionSkillLevel(skillModifier.Key), Inventory.Player.Skills.GetItemSkillLevel(skillModifier.Key) - skillModifier.Value) );
                }
            }

            if (ToItem.Metadata.SlotType == SlotType.Head && (Slot)slot == Slot.Head ||
                ToItem.Metadata.SlotType == SlotType.Necklace && (Slot)slot == Slot.Necklace ||
                ToItem.Metadata.SlotType == SlotType.Backpack && (Slot)slot == Slot.Backpack ||
                ToItem.Metadata.SlotType == SlotType.Body && (Slot)slot == Slot.Body ||
                (ToItem.Metadata.SlotType == SlotType.Hand || ToItem.Metadata.SlotType == SlotType.TwoHanded) && ( (Slot)slot == Slot.Left || (Slot)slot == Slot.Right) ||
                ToItem.Metadata.SlotType == SlotType.Feet && (Slot)slot == Slot.Feet ||
                ToItem.Metadata.SlotType == SlotType.Ring && (Slot)slot == Slot.Ring ||
                ToItem.Metadata.SlotType == SlotType.Ammo && (Slot)slot == Slot.Ammo)
            {
                if (ToItem.Metadata.OpenTibiaId == stealthRing)
                {
                    await Context.AddCommand(new CreatureUpdateOutfitCommand(Inventory.Player, Inventory.Player.BaseOutfit, Inventory.Player.ConditionOutfit, Inventory.Player.Swimming, Inventory.Player.ConditionStealth, true) );
                }

                if (ToItem.Metadata.SpeedModifier != null)
                {
                    await Context.AddCommand(new CreatureUpdateSpeedCommand(Inventory.Player, Inventory.Player.ConditionSpeed, Inventory.Player.ItemSpeed + ToItem.Metadata.SpeedModifier.Value) );
                }

                foreach (var skillModifier in ToItem.Metadata.SkillModifier)
                {
                    await Context.AddCommand(new PlayerUpdateSkillCommand(Inventory.Player, skillModifier.Key, Inventory.Player.Skills.GetSkillPoints(skillModifier.Key), Inventory.Player.Skills.GetSkillLevel(skillModifier.Key), Inventory.Player.Skills.GetSkillPercent(skillModifier.Key), Inventory.Player.Skills.GetConditionSkillLevel(skillModifier.Key), Inventory.Player.Skills.GetItemSkillLevel(skillModifier.Key) + skillModifier.Value) );
                }
            }

            Context.AddEvent(new InventoryReplaceItemEventArgs(Inventory, FromItem, ToItem, slot) );
        }
    }
}