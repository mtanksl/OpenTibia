using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RingEquipHandler : EventHandlers.EventHandler<InventoryAddItemEventArgs>
    {
        private readonly Dictionary<ushort, ushort> ringEquip;
        private readonly HashSet<ushort> stealthRing;
        private readonly HashSet<ushort> lifeRing;
        private readonly HashSet<ushort> ringOfHealing;
        private readonly HashSet<ushort> energyRing;
        private readonly HashSet<ushort> timeRing;
        private readonly HashSet<ushort> dwarvenRing;

        public RingEquipHandler()
        {
            ringEquip = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.ringEquip");
            stealthRing = Context.Server.Values.GetUInt16HashSet("values.items.stealthRing");
            lifeRing = Context.Server.Values.GetUInt16HashSet("values.items.lifeRing");
            ringOfHealing = Context.Server.Values.GetUInt16HashSet("values.items.ringOfHealing");
            energyRing = Context.Server.Values.GetUInt16HashSet("values.items.energyRing");
            timeRing = Context.Server.Values.GetUInt16HashSet("values.items.timeRing");
            dwarvenRing = Context.Server.Values.GetUInt16HashSet("values.items.dwarvenRing");
        }

        public override Promise Handle(InventoryAddItemEventArgs e)
        {
            ushort toOpenTibiaId;

            if (ringEquip.TryGetValue(e.Item.Metadata.OpenTibiaId, out toOpenTibiaId) && (Slot)e.Slot == Slot.Ring)
            {
                if (stealthRing.Contains(toOpenTibiaId) )
                {
                    e.Inventory.Player.StealthRing = true;

                    foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(e.Inventory.Player.Tile.Position) )
                    {
                        byte clientIndex;

                        if (observer.Client.TryGetIndex(e.Inventory.Player, out clientIndex) )
                        {
                            Context.AddPacket(observer, new SetOutfitOutgoingPacket(e.Inventory.Player.Id, e.Inventory.Player.Outfit) );
                        }
                    }
                }
                else if (lifeRing.Contains(toOpenTibiaId) )
                {
                    //TODO
                }
                else if (ringOfHealing.Contains(toOpenTibiaId) )
                {
                    //TODO
                }
                else if (energyRing.Contains(toOpenTibiaId) )
                {
                    //TODO
                }
                else if (timeRing.Contains(toOpenTibiaId) )
                {
                    //TODO
                }
                else if (dwarvenRing.Contains(toOpenTibiaId) )
                {
                    //TODO
                }

                return Context.AddCommand(new ItemTransformCommand(e.Item, toOpenTibiaId, 1) );
            }

            return Promise.Completed;
        }
    }
}