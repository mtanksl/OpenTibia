using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BabySealDollHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> partyTrumpets = new Dictionary<ushort, ushort>() 
        {
            { 7183, 7184 }
        };

        private Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>() 
        {
            { 7184, 7183 }
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (partyTrumpets.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                command.Player.Client.Storages.SetValue(AchievementConstants.INeedAHugBabySealDoll, 1);

                if (command.Player.Client.Storages.TryGetValue(AchievementConstants.INeedAHugPandaTeddy, out _) &&
                    command.Player.Client.Storages.TryGetValue(AchievementConstants.INeedAHugStuffedDragon, out _) &&
                    command.Player.Client.Storages.TryGetValue(AchievementConstants.INeedAHugBabySealDoll, out _) &&
                    command.Player.Client.Storages.TryGetValue(AchievementConstants.INeedAHugSantaDoll, out _))
                {
                    if ( !command.Player.Client.Achievements.HasAchievement("I Need a Hug") )
                    {
                        command.Player.Client.Achievements.SetAchievement("I Need a Hug");

                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "Congratulations! You earned the achievement \"I Need a Hug\".") );
                    }
                }

                return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) ).Then( (item) =>
                {
                    _ = Context.AddCommand(new ItemDecayTransformCommand(item, TimeSpan.FromSeconds(3), decay[item.Metadata.OpenTibiaId], 1));

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}