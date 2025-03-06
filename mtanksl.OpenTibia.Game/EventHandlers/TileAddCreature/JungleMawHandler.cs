using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class JungleMawHandler : EventHandlers.EventHandler<TileAddCreatureEventArgs>
    {
        private readonly Dictionary<ushort, ushort> jungleMaws;

        public JungleMawHandler()
        {
            jungleMaws = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.jungleMaws");
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
                        return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Creature, new DamageAttack(null, MagicEffectType.BlackSpark, DamageType.Physical, 30, 30, false) ) ).Then( () =>
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