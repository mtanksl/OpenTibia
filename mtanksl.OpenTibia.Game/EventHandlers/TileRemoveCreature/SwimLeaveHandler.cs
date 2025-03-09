using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SwimLeaveHandler : EventHandlers.EventHandler<TileRemoveCreatureEventArgs>
    {
        private readonly HashSet<ushort> shallowWaters;

        public SwimLeaveHandler()
        {
            shallowWaters = Context.Server.Values.GetUInt16HashSet("values.items.shallowWaters");
        }

        public override Promise Handle(TileRemoveCreatureEventArgs e)
        {
            Tile fromTile = e.FromTile;

            Tile toTile = e.ToTile;

            if (fromTile != null && toTile == null)
            {
                if (fromTile.Ground != null)
                {
                    if (shallowWaters.Contains(fromTile.Ground.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new CreatureUpdateOutfitCommand(e.Creature, e.Creature.BaseOutfit, e.Creature.ConditionOutfit, false, e.Creature.ConditionStealth, e.Creature.ItemStealth, e.Creature.IsMounted) );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}