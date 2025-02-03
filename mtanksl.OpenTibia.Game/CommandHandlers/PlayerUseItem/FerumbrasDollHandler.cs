using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FerumbrasDollHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> ferumbrasDolls;

        public FerumbrasDollHandler()
        {
            ferumbrasDolls = Context.Server.Values.GetUInt16HashSet("values.items.ferumbrasDolls");
        }

        private static List<string> sounds = new List<string>() { "NO ONE WILL STOP ME THIS TIME!", "THE POWER IS MINE!", "THE POWER IS IN TIBIOPEDIA!", "Mwahaha!" };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (ferumbrasDolls.Contains(command.Item.Metadata.OpenTibiaId) )
            {                  
                int value = Context.Server.Randomization.Take(0, sounds.Count - 1);

                return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, sounds[value] ) );
            }

            return next();
        }
    }

    
}