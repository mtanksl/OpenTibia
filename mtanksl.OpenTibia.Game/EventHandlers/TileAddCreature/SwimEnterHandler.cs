using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SwimEnterHandler : EventHandlers.EventHandler<TileAddCreatureEventArgs>
    {
        private HashSet<ushort> shallowWaters = new HashSet<ushort>() { 4608, 4609, 4610, 4611, 4612, 4613, 4614, 4615, 4616, 4617, 4618, 4619, 4620, 4621, 4622, 4623, 4624, 4625, 4820, 4821, 4822, 4823, 4824, 4825 };

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            if ( !e.Creature.Invisible)
            {
                Tile fromTile = e.FromTile;

                Tile toTile = e.ToTile;

                if (fromTile == null && toTile != null)
                {
                    if (toTile.Ground != null)
                    {
                        if (shallowWaters.Contains(toTile.Ground.Metadata.OpenTibiaId) )
                        {
                            return Context.AddCommand(new CreatureUpdateOutfitCommand(e.Creature, e.Creature.BaseOutfit, Outfit.Swimming) ).Then( () =>
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
                            return Context.AddCommand(new CreatureUpdateOutfitCommand(e.Creature, e.Creature.BaseOutfit, Outfit.Swimming) ).Then( () =>
                            {
                                return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.WaterSplash) );
                            } );
                        }
                        else if (shallowWaters.Contains(fromTile.Ground.Metadata.OpenTibiaId) && !shallowWaters.Contains(toTile.Ground.Metadata.OpenTibiaId) )
                        {
                            return Context.AddCommand(new CreatureUpdateOutfitCommand(e.Creature, e.Creature.BaseOutfit, e.Creature.BaseOutfit) );
                        }
                    }
                }
            }

            return Promise.Completed;
        }
    }
}