using OpenTibia.Common.Objects;
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
            var currentLight = Inventory.Player.ClientLight;

            byte slot = (byte)Inventory.GetIndex(FromItem);

            Inventory.ReplaceContent(slot, ToItem);

            Context.AddPacket(Inventory.Player, new SlotAddOutgoingPacket(slot, ToItem ) );

            var maxLightLevel = Inventory.GetItems()
                .Select(i => i.Metadata.Light)
                .Where(l => l != null)
                .OrderByDescending(l => l.Level)
                .FirstOrDefault();

            await Context.AddCommand(new CreatureUpdateLightCommand(Inventory.Player, Inventory.Player.ConditionLight, maxLightLevel) );

            if (FromItem.Metadata.OpenTibiaId == stealthRing && (OpenTibia.Common.Structures.Slot)slot == OpenTibia.Common.Structures.Slot.Ring)
            {
                await Context.AddCommand(new CreatureUpdateOutfitCommand(Inventory.Player, Inventory.Player.BaseOutfit, Inventory.Player.ConditionOutfit, Inventory.Player.Swimming, Inventory.Player.ConditionStealth, false) );
            }

            if (FromItem.Metadata.SpeedModifier != null)
            {
                await Context.AddCommand(new CreatureUpdateSpeedCommand(Inventory.Player, Inventory.Player.ConditionSpeed, 0) );
            }

            foreach (var skillModifier in FromItem.Metadata.SkillModifier)
            {
                await Context.AddCommand(new PlayerUpdateSkillCommand(Inventory.Player, skillModifier.Key, Inventory.Player.Skills.GetSkillPoints(skillModifier.Key), Inventory.Player.Skills.GetClientSkillLevel(skillModifier.Key), Inventory.Player.Skills.GetSkillPercent(skillModifier.Key), Inventory.Player.Skills.GetConditionSkillLevel(skillModifier.Key), 0) );
            }
            
            if (ToItem.Metadata.OpenTibiaId == stealthRing && (OpenTibia.Common.Structures.Slot)slot == OpenTibia.Common.Structures.Slot.Ring)
            {
                await Context.AddCommand(new CreatureUpdateOutfitCommand(Inventory.Player, Inventory.Player.BaseOutfit, Inventory.Player.ConditionOutfit, Inventory.Player.Swimming, Inventory.Player.ConditionStealth, true) );
            }

            if (ToItem.Metadata.SpeedModifier != null)
            {
                await Context.AddCommand(new CreatureUpdateSpeedCommand(Inventory.Player, Inventory.Player.ConditionSpeed, ToItem.Metadata.SpeedModifier.Value) );
            }

            foreach (var skillModifier in ToItem.Metadata.SkillModifier)
            {
                await Context.AddCommand(new PlayerUpdateSkillCommand(Inventory.Player, skillModifier.Key, Inventory.Player.Skills.GetSkillPoints(skillModifier.Key), Inventory.Player.Skills.GetClientSkillLevel(skillModifier.Key), Inventory.Player.Skills.GetSkillPercent(skillModifier.Key), Inventory.Player.Skills.GetConditionSkillLevel(skillModifier.Key), skillModifier.Value) );
            }

            Context.AddEvent(new InventoryReplaceItemEventArgs(Inventory, FromItem, ToItem, slot) );
        }
    }
}