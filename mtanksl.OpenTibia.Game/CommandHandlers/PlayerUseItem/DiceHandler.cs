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

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemCommand command)
        {
            if (dices.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                int value = context.Server.Randomization.Take(0, 6);

                ushort openTibiaId = (ushort)(5792 + value);

                switch (command.Item.Root() )
                {
                    case Tile tile:

                        context.AddCommand(new ShowMagicEffectCommand(tile.Position, MagicEffectType.Dice) );

                        break;

                    case Inventory inventory:

                        context.AddCommand(new ShowMagicEffectCommand(inventory.Player.Tile.Position, MagicEffectType.Dice) );

                        break;
                }
                               
                context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, command.Player.Name + " rolled a " + (value + 1) + ".") );

                context.AddCommand(new ItemTransformCommand(command.Item, openTibiaId, 1) );

                return Promise.FromResult(context);
            }

            return next(context);
        }
    }
}