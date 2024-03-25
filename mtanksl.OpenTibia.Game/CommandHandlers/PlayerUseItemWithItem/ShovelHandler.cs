using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ShovelHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> shovels = new HashSet<ushort>() { 2554, 5710, 10513, 10515, 10511 };

        private Dictionary<ushort, ushort> stonePiles = new Dictionary<ushort, ushort>()
        {
            { 468, 469 },
            { 481, 482 },
            { 483, 484 }
        };

        private Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>()
        {
            { 469, 468 },
            { 482, 481 },
            { 484, 483 }
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (shovels.Contains(command.Item.Metadata.OpenTibiaId) && stonePiles.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                int count;

                command.Player.Client.Storages.TryGetValue(AchievementConstants.TheUndertaker, out count);

                command.Player.Client.Storages.SetValue(AchievementConstants.TheUndertaker, ++count);

                if (count >= 500)
                {
                    if ( !command.Player.Client.Achievements.HasAchievement("The Undertaker") )
                    {
                        command.Player.Client.Achievements.SetAchievement("The Undertaker");

                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "Congratulations! You earned the achievement \"The Undertaker\".") );
                    }
                }

                return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) ).Then( (item) =>
                {
                    _ = Context.AddCommand(new ItemDecayTransformCommand(item, TimeSpan.FromSeconds(10), decay[item.Metadata.OpenTibiaId], 1) );

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}