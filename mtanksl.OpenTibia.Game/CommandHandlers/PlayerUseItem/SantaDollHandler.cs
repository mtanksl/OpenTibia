using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SantaDollHandler : CommandHandler<PlayerUseItemCommand>
    {
        private static HashSet<ushort> santaDolls = new HashSet<ushort>() { 6567 };

        private static List<string> sounds = new List<string>() { "Ho ho ho!", "Jingle bells, jingle bells...", "Have you been naughty?", "Have you been nice?", "Merry Christmas!", "Can you stop squeezing me now... I'm starting to feel a little sick." };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (santaDolls.Contains(command.Item.Metadata.OpenTibiaId) )
            {                
                int value = Context.Server.Randomization.Take(0, sounds.Count - 1);

                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.INeedAHugSantaDoll, new[] { AchievementConstants.INeedAHugPandaTeddy, AchievementConstants.INeedAHugStuffedDragon, AchievementConstants.INeedAHugBabySealDoll, AchievementConstants.INeedAHugSantaDoll }, "I Need a Hug") ).Then( () =>
                {
                    return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, sounds[value] ) );
                } );
            }

            return next();
        }
    }
}