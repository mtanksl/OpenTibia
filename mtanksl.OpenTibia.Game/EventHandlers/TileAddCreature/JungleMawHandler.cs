using OpenTibia.Common.Events;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class JungleMawHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private Dictionary<ushort, ushort> jungleMaws = new Dictionary<ushort, ushort>()
        {
            { 4208, 4209 }
        };

        private Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>() 
        {
            { 4209, 4208 }
        };

        public override void Handle(Context context, TileAddCreatureEventArgs e)
        {
            foreach (var topItem in e.Tile.GetItems() )
            {
                ushort toOpenTibiaId;

                if (jungleMaws.TryGetValue(topItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    context.AddCommand(new CombatTargetedAttackCommand(null, e.Creature, null, MagicEffectType.BlackSpark, target => -30) );

                    context.AddCommand(new ItemTransformCommand(topItem, toOpenTibiaId, 1) ).Then( (ctx, item) =>
                    {
                        return ctx.AddCommand(new ItemDecayTransformCommand(item, 10000, decay[toOpenTibiaId], 1) );
                    } );

                    break;
                }
            }
        }
    }
}