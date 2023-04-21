using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SpikesHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private ushort activeSpike = 1513;

        private Dictionary<ushort, ushort> spikes = new Dictionary<ushort, ushort>()
        {
            { 1512, 1513 }
        };

        private Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>()
        {
            { 1513, 1512 }
        };

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            foreach (var topItem in e.Tile.GetItems() )
            {
                ushort toOpenTibiaId;

                if (spikes.TryGetValue(topItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Creature, new SimpleAttack(null, MagicEffectType.BlackSpark, AnimatedTextColor.DarkRed, -60) ) ).Then( () =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(topItem, toOpenTibiaId, 1) );

                    } ).Then( (item) =>
                    {
                        _ = Context.AddCommand(new ItemDecayTransformCommand(item, 10000, decay[toOpenTibiaId], 1) );

                        return Promise.Completed;
                    } );
                }
                else if (topItem.Metadata.OpenTibiaId == activeSpike)
                {
                    return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Creature, new SimpleAttack(null, MagicEffectType.BlackSpark, AnimatedTextColor.DarkRed, -60) ) );
                }
            }

            return Promise.Completed;
        }
    }
}