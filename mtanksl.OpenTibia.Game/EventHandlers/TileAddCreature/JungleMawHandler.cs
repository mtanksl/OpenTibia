using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class JungleMawHandler : EventHandlers.EventHandler<TileAddCreatureEventArgs>
    {
        private readonly Dictionary<ushort, ushort> jungleMaws;
        private readonly Dictionary<ushort, ushort> decay;

        public JungleMawHandler()
        {
            jungleMaws = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.jungleMaws");
            decay = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.decay.jungleMaws");
        }        

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            if (e.ToTile.Field)
            {
                foreach (var topItem in e.ToTile.GetItems() )
                {
                    ushort toOpenTibiaId;

                    if (jungleMaws.TryGetValue(topItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                    {
                        return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Creature, new DamageAttack(null, MagicEffectType.BlackSpark, DamageType.Physical, 30, 30) ) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(topItem, toOpenTibiaId, 1) );

                        } ).Then( (item) =>
                        {
                            _ = Context.AddCommand(new ItemDecayTransformCommand(item, TimeSpan.FromSeconds(10), decay[toOpenTibiaId], 1) );

                            return Promise.Completed;
                        } );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}