using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.CommandHandlers
{
    public class SpikesHandler : EventHandlers.EventHandler<TileAddCreatureEventArgs>
    {
        private readonly ushort activeSpike;

        public SpikesHandler()
        {
            activeSpike = Context.Server.Values.GetUInt16("values.items.activeSpike");
        }

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            if (e.ToTile.Field)
            {
                foreach (var topItem in e.ToTile.GetItems() )
                {
                    if (topItem.Metadata.OpenTibiaId == activeSpike)
                    {
                        return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Creature, new DamageAttack(null, MagicEffectType.BlackSpark, DamageType.Physical, 60, 60, false) ) );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}