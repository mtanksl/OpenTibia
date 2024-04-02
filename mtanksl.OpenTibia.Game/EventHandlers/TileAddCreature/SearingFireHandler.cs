using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SearingFireHandler : EventHandler<TileAddCreatureEventArgs>
    {
        private HashSet<ushort> searingFires = new HashSet<ushort>() { 1506, 1507 };

        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            foreach (var topItem in e.ToTile.GetItems() )
            {
                if (searingFires.Contains(topItem.Metadata.OpenTibiaId) )
                {
                    return Context.AddCommand(new CreatureAttackCreatureCommand(null, e.Creature,

                        new SimpleAttack(null, MagicEffectType.FirePlume, AnimatedTextColor.Orange, 300, 300) ) );
                }
            }

            return Promise.Completed;
        }
    }
}