using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class OpenTrapHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private readonly Dictionary<ushort, ushort> traps;

        public OpenTrapHandler()
        {
            traps = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.decay.traps");
        }

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            if (e.ToTile.Field)
            {
                foreach (var topItem in e.ToTile.GetItems() )
                {
                    ushort toOpenTibiaId;

                    if (traps.TryGetValue(topItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                    {
                        return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Creature, new DamageAttack(null, MagicEffectType.BlackSpark, DamageType.Physical, 30, 30) ) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(topItem, toOpenTibiaId, 1) );
                        } );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}