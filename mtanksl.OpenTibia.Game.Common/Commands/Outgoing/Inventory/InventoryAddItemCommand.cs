using OpenTibia.Common.Objects;
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

            Slot = slot;

            Item = item;
        }

        public Inventory Inventory { get; set; }

        public byte Slot { get; set; }

        public Item Item { get; set; }

        public override async Promise Execute()
        {
            var currentLight = Inventory.Player.ClientLight;

            Inventory.AddContent(Item, Slot);

            Context.AddPacket(Inventory.Player, new SlotAddOutgoingPacket(Slot, Item ) );

            var maxLightLevel = Inventory.GetItems()
                .Select(i => i.Metadata.Light)
                .Where(l => l != null)
                .OrderByDescending(l => l.Level)
                .FirstOrDefault();

            await Context.AddCommand(new CreatureUpdateLightCommand(Inventory.Player, Inventory.Player.ConditionLight, maxLightLevel) );

            if (Item.Metadata.OpenTibiaId == stealthRing && (OpenTibia.Common.Structures.Slot)Slot == OpenTibia.Common.Structures.Slot.Ring)
            {
                await Context.AddCommand(new CreatureUpdateOutfitCommand(Inventory.Player, Inventory.Player.BaseOutfit, Inventory.Player.ConditionOutfit, Inventory.Player.Swimming, Inventory.Player.ConditionStealth, true) );
            }

            if (Item.Metadata.SpeedModifier != null)
            {
                await Context.AddCommand(new CreatureUpdateSpeedCommand(Inventory.Player, Inventory.Player.ConditionSpeed, Item.Metadata.SpeedModifier.Value) );
            }

            foreach (var skillModifier in Item.Metadata.SkillModifier)
            {
                await Context.AddCommand(new PlayerUpdateSkillCommand(Inventory.Player, skillModifier.Key, Inventory.Player.Skills.GetSkillPoints(skillModifier.Key), Inventory.Player.Skills.GetClientSkillLevel(skillModifier.Key), Inventory.Player.Skills.GetSkillPercent(skillModifier.Key), Inventory.Player.Skills.GetConditionSkillLevel(skillModifier.Key), skillModifier.Value) );
            }            

            Context.AddEvent(new InventoryAddItemEventArgs(Inventory, Item, Slot) );
        }
    }
}