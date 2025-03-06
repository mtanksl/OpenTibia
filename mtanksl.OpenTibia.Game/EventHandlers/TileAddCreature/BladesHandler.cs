using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.CommandHandlers
{
    public class BladesHandler : EventHandlers.EventHandler<TileAddCreatureEventArgs>
    {
        private readonly ushort activeBlade;

        public BladesHandler()
        {
            activeBlade = Context.Server.Values.GetUInt16("values.items.activeBlade");
        }

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            if (e.ToTile.Field)
            {
                foreach (var topItem in e.ToTile.GetItems() )
                {
                    if (topItem.Metadata.OpenTibiaId == activeBlade)
                    {
                        return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Creature, new DamageAttack(null, MagicEffectType.BlackSpark, DamageType.Physical, 60, 60, false) ) );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}