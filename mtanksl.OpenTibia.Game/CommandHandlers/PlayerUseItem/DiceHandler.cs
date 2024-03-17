using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class DiceHandler : CommandHandler<PlayerUseItemCommand>
    {
        private ushort dice = 5792;

        private HashSet<ushort> dices = new HashSet<ushort>() { 5792, 5793, 5794, 5795, 5796, 5797 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (dices.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                int value = Context.Server.Randomization.Take(0, dices.Count - 1);

                switch (command.Item.Root() )
                {
                    case Tile tile:

                        return Context.AddCommand(new ShowMagicEffectCommand(tile.Position, MagicEffectType.Dice) ).Then( () =>
                        {
                            return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, command.Player.Name + " rolled a " + (value + 1) + ".") );

                        } ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.Item, (ushort)(dice + value), 1) );
                        } );

                    case Inventory inventory:
                    case Safe safe:

                        return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Dice) ).Then( () =>
                        {
                            return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, command.Player.Name + " rolled a " + (value + 1) + ".") );

                        } ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.Item, (ushort)(dice + value), 1) );
                        } );
                }
            }

            return next();
        }
    }
}