using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class NorsemanDollHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> norsemanDolls;

        public NorsemanDollHandler()
        {
            norsemanDolls = Context.Server.Values.GetUInt16HashSet("values.items.norsemanDolls");
        }

        private static List<string> sounds = new List<string>() { "Hail TibiaNordic!", "So cold...", "Run, mammoth!" };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (norsemanDolls.Contains(command.Item.Metadata.OpenTibiaId) )
            {                  
                int value = Context.Server.Randomization.Take(0, sounds.Count - 1);

                return Context.AddCommand(new ShowTextCommand(command.Player, MessageMode.MonsterSay, sounds[value] ) );
            }

            return next();
        }
    }

    
}