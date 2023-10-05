using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class HelmetOfTheDeepHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private HashSet<ushort> oceanFloors = new HashSet<ushort>() { 5405, 5406, 5407, 5408, 5409, 5410 };

        private HashSet<ushort> helmetOfTheDeeps = new HashSet<ushort>() { 5461 };

        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (helmetOfTheDeeps.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (command.ToContainer is Inventory toInventory && (Slot)command.ToIndex == Slot.Head)
                {
                    if (command.Player.HasSpecialCondition(SpecialCondition.Drowning) )
                    {
                        return next().Then( () =>
                        {
                            return Context.AddCommand(new CreatureRemoveConditionCommand(command.Player, ConditionSpecialCondition.Drowning) );
                        } );
                    }
                }
                else if (command.Item.Parent is Inventory fromInventory && (Slot)fromInventory.GetIndex(command.Item) == Slot.Head)
                {
                    if (oceanFloors.Contains(command.Player.Tile.Ground.Metadata.OpenTibiaId) )
                    {
                        if ( !command.Player.HasSpecialCondition(SpecialCondition.Drowning) )
                        {
                            return next().Then( () =>
                            {
                                return Context.AddCommand(new CreatureAttackCreatureCommand(null, command.Player, 

                                    new SimpleAttack(null, MagicEffectType.BlueRings, AnimatedTextColor.Crystal, 20, 20),

                                    new DrowningCondition(20, TimeSpan.FromSeconds(4) ) ) );
                            } );
                        }
                    }
                }
            }

            return next();
        }
    }
}