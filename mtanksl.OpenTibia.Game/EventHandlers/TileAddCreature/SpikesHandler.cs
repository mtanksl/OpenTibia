using OpenTibia.Common.Events;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
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

        public override void Handle(Context context, TileAddCreatureEventArgs e)
        {
            foreach (var topItem in e.Tile.GetItems() )
            {
                ushort toOpenTibiaId;

                if (spikes.TryGetValue(topItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    context.AddCommand(new CombatDirectAttackCommand(e.Creature, MagicEffectType.BlackSpark, -60) );

                    context.AddCommand(new ItemTransformCommand(topItem, toOpenTibiaId, 1) ).Then( (ctx, item) =>
                    {
                        return ctx.AddCommand(new ItemDecayTransformCommand(item, 10000, decay[toOpenTibiaId], 1) );
                    } );

                    break;
                }
                else if (topItem.Metadata.OpenTibiaId == activeSpike)
                {
                    context.AddCommand(new CombatDirectAttackCommand(e.Creature, MagicEffectType.BlackSpark, -60) );

                    break;
                }
            }
        }
    }
}