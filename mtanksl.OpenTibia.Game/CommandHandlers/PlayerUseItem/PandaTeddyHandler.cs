using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PandaTeddyHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> pandaTeddies = new HashSet<ushort>() { 5080 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (pandaTeddies.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                int count;

                command.Player.Client.Storages.TryGetValue(AchievementConstants.INeedAHugPandaTeddy, out count);

                command.Player.Client.Storages.SetValue(AchievementConstants.INeedAHugPandaTeddy, ++count);

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

                return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "Hug me!") );
            }

            return next();
        }
    }
}