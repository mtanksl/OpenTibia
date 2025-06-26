using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class EpaminondasDollHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> epaminondasDolls;

        public EpaminondasDollHandler()
        {
            epaminondasDolls = Context.Server.Values.GetUInt16HashSet("values.items.epaminondasDolls");
        }

        private static List<string> sounds = new List<string>() { "Hail!", "Hail Portal Tibia!", "Hauopa!", "WHERE IS MY HYDROMEL?!", "Yala Boom" };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (epaminondasDolls.Contains(command.Item.Metadata.OpenTibiaId) )
            {                  
                int value = Context.Server.Randomization.Take(0, sounds.Count - 1);

                return Context.AddCommand(new ShowTextCommand(command.Player, MessageMode.MonsterSay, sounds[value] ) ).Then( () =>
                {
                    if (value == sounds.Count - 1)
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.RedNotes) );
                    }

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}