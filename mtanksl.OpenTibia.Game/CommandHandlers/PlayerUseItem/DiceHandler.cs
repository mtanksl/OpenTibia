﻿using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class DiceHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> dices;
        private readonly ushort dice;

        public DiceHandler()
        {
            dices = Context.Server.Values.GetUInt16HashSet("values.items.dices");
            dice = Context.Server.Values.GetUInt16("values.items.dice");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (dices.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                int value = Context.Server.Randomization.Take(0, dices.Count - 1);

                return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.Dice) ).Then( () =>
                {
                    return Context.AddCommand(new ShowTextCommand(command.Player, MessageMode.MonsterSay, command.Player.Name + " rolled a " + (value + 1) + ".") );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.Item, (ushort)(dice + value), 1) );
                } );
            }

            return next();
        }
    }
}