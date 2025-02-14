using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SwimEnterHandler : EventHandlers.EventHandler<TileAddCreatureEventArgs>
    {
        private readonly HashSet<ushort> shallowWaters;

        public SwimEnterHandler()
        {
            shallowWaters = Context.Server.Values.GetUInt16HashSet("values.items.shallowWaters");
        }

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            Tile fromTile = e.FromTile;

            Tile toTile = e.ToTile;

            if (fromTile == null && toTile != null)
            {
                if (toTile.Ground != null)
                {
                    if (shallowWaters.Contains(toTile.Ground.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new CreatureUpdateOutfitCommand(e.Creature, e.Creature.BaseOutfit, e.Creature.ConditionOutfit, true, e.Creature.ConditionStealth) ).Then( () =>
                        {
                            return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.WaterSplash) );
                        } );
                    }
                }
            }
            else if (fromTile != null && toTile != null)
            {
                if (fromTile.Ground != null && toTile.Ground != null)
                {
                    if ( !shallowWaters.Contains(fromTile.Ground.Metadata.OpenTibiaId) && shallowWaters.Contains(toTile.Ground.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new CreatureUpdateOutfitCommand(e.Creature, e.Creature.BaseOutfit, e.Creature.ConditionOutfit, true, e.Creature.ConditionStealth) ).Then( () =>
                        {
                            return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.WaterSplash) );
                        } );
                    }
                    else if (shallowWaters.Contains(fromTile.Ground.Metadata.OpenTibiaId) && !shallowWaters.Contains(toTile.Ground.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new CreatureUpdateOutfitCommand(e.Creature, e.Creature.BaseOutfit, e.Creature.ConditionOutfit, false, e.Creature.ConditionStealth) );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}