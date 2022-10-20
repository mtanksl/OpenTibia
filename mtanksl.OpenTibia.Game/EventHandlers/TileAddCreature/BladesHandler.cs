using OpenTibia.Common.Events;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BladesHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private ushort activeBlade = 1511;

        private Dictionary<ushort, ushort> blades = new Dictionary<ushort, ushort>()
        {
            { 1510, 1511 }
        };

        private Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>()
        {
            { 1511, 1510 }
        };

        public override void Handle(Context context, TileAddCreatureEventArgs e)
        {
            foreach (var topItem in e.Tile.GetItems() )
            {
                ushort toOpenTibiaId;

                if (blades.TryGetValue(topItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    context.AddCommand(new CombatTargetedAttackCommand(null, e.Creature, null, MagicEffectType.BlackSpark, (attacker, target) => -60) );

                    context.AddCommand(new ItemTransformCommand(topItem, toOpenTibiaId, 1) ).Then( (ctx, item) =>
                    {
                        return ctx.AddCommand(new ItemDecayTransformCommand(item, 10000, decay[toOpenTibiaId], 1) );
                    } );

                    break;
                }
                else if (topItem.Metadata.OpenTibiaId == activeBlade)
                {
                    context.AddCommand(new CombatTargetedAttackCommand(null, e.Creature, null, MagicEffectType.BlackSpark, (attacker, target) => -60) );

                    break;
                }
            }
        }
    }
}