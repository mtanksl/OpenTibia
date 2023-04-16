using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
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

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            foreach (var topItem in e.Tile.GetItems() )
            {
                ushort toOpenTibiaId;

                if (blades.TryGetValue(topItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    return Context.AddCommand(new CombatAttackCreatureWithEnvironmentCommand(e.Creature, MagicEffectType.BlackSpark, -60) ).Then( () =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(topItem, toOpenTibiaId, 1) );

                    } ).Then( (item) =>
                    {
                         _ = Context.AddCommand(new ItemDecayTransformCommand(item, 10000, decay[toOpenTibiaId], 1) );

                        return Promise.Completed;
                    } );
                }
                else if (topItem.Metadata.OpenTibiaId == activeBlade)
                {
                    return Context.AddCommand(new CombatAttackCreatureWithEnvironmentCommand(e.Creature, MagicEffectType.BlackSpark, -60) );
                }
            }

            return Promise.Completed;
        }
    }
}