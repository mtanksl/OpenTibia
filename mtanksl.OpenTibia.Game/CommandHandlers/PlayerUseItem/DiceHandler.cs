using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class DiceHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> dices = new HashSet<ushort>() { 5792, 5793, 5794, 5795, 5796, 5797 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (dices.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                int value = Context.Server.Randomization.Take(0, dices.Count);

                ushort openTibiaId = (ushort)(5792 + value);

                switch (command.Item.Root() )
                {
                    case Tile tile:

                        Context.AddCommand(new ShowMagicEffectCommand(tile.Position, MagicEffectType.Dice) );

                        break;

                    case Inventory inventory:

                        Context.AddCommand(new ShowMagicEffectCommand(inventory.Player.Tile.Position, MagicEffectType.Dice) );

                        break;
                }
                               
                Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, command.Player.Name + " rolled a " + (value + 1) + ".") );

                Context.AddCommand(new ItemTransformCommand(command.Item, openTibiaId, 1) );

                return Promise.Completed();
            }

            return next();
        }
    }
}