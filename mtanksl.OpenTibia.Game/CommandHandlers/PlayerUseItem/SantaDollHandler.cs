using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
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
                int count;

                command.Player.Client.Storages.TryGetValue(AchievementConstants.INeedAHugSantaDoll, out count);

                command.Player.Client.Storages.SetValue(AchievementConstants.INeedAHugSantaDoll, ++count);

                if (command.Player.Client.Storages.TryGetValue(AchievementConstants.INeedAHugPandaTeddy, out _) &&
                    command.Player.Client.Storages.TryGetValue(AchievementConstants.INeedAHugStuffedDragon, out _) &&
                    command.Player.Client.Storages.TryGetValue(AchievementConstants.INeedAHugBabySealDoll, out _) &&
                    command.Player.Client.Storages.TryGetValue(AchievementConstants.INeedAHugSantaDoll, out _) )
                {
                    if ( !command.Player.Client.Achievements.HasAchievement("I Need a Hug") )
                    {
                        command.Player.Client.Achievements.SetAchievement("I Need a Hug");

                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "Congratulations! You earned the achievement \"I Need a Hug\".") );
                    }
                }

                int value = Context.Server.Randomization.Take(0, sounds.Count - 1);

                return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, sounds[value] ) );
            }

            return next();
        }
    }
}