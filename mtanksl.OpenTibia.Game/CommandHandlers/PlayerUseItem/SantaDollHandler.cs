using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SantaDollHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> stuffedDragons = new HashSet<ushort>() { 6567 };

        private List<string> sounds = new List<string>() { "Ho ho ho!", "Jingle bells, jingle bells...", "Have you been naughty?", "Have you been nice?", "Merry Christmas!", "Can you stop squeezing me now... I'm starting to feel a little sick." };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (stuffedDragons.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                int value = Context.Server.Randomization.Take(0, sounds.Count);

                Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, sounds[value] ) );

                return Promise.Completed();
            }

            return next();
        }
    }
}