using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SpikesHandler : EventHandlers.EventHandler<TileAddCreatureEventArgs>
    {
        private readonly ushort activeSpike;
        private readonly Dictionary<ushort, ushort> spikes;
        private readonly Dictionary<ushort, ushort> decay;

        public SpikesHandler()
        {
            activeSpike = Context.Server.Values.GetUInt16("values.items.activeSpike");
            spikes = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.spikes");
            decay = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.decay.spikes");
        }

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            if (e.ToTile.Field)
            {
                foreach (var topItem in e.ToTile.GetItems() )
                {
                    ushort toOpenTibiaId;

                    if (spikes.TryGetValue(topItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                    {
                        return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Creature, new DamageAttack(null, MagicEffectType.BlackSpark, DamageType.Physical, 60, 60, false) ) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(topItem, toOpenTibiaId, 1) );

                        } ).Then( (item) =>
                        {
                            _ = Context.AddCommand(new ItemDecayTransformCommand(item, TimeSpan.FromSeconds(10), decay[toOpenTibiaId], 1) );

                            return Promise.Completed;
                        } );
                    }
                    else if (topItem.Metadata.OpenTibiaId == activeSpike)
                    {
                        return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Creature, new DamageAttack(null, MagicEffectType.BlackSpark, DamageType.Physical, 60, 60, false) ) );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}