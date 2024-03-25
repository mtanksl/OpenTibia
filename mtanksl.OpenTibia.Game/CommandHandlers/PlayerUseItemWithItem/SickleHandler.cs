using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SickleHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> sickles = new HashSet<ushort>() { 2405, 2418, 10513 };

        private Dictionary<ushort, ushort> wheats = new Dictionary<ushort, ushort>()
        {
            { 5471, 5463 }
        };

        private Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>()
        {
            { 5463, 5464 },
            { 5464, 5466 }
        };

        private ushort bunchOfSugarCane = 5467;

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (sickles.Contains(command.Item.Metadata.OpenTibiaId) && wheats.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                int count;

                command.Player.Client.Storages.TryGetValue(AchievementConstants.NaturalSweetener, out count);

                command.Player.Client.Storages.SetValue(AchievementConstants.NaturalSweetener, ++count);

                if (count >= 50)
                {
                    if ( !command.Player.Client.Achievements.HasAchievement("Natural Sweetener") )
                    {
                        command.Player.Client.Achievements.SetAchievement("Natural Sweetener");

                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "Congratulations! You earned the achievement \"Natural Sweetener\".") );
                    }
                }

                return Context.AddCommand(new TileCreateItemOrIncrementCommand( (Tile)command.ToItem.Parent, bunchOfSugarCane, 1) ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );

                } ).Then( (item) =>
                {
                    _ = Context.AddCommand(new ItemDecayTransformCommand(item, TimeSpan.FromSeconds(10), decay[item.Metadata.OpenTibiaId], 1) ).Then( (item2) =>
                    {
                        _ = Context.AddCommand(new ItemDecayTransformCommand(item2, TimeSpan.FromSeconds(10), decay[item2.Metadata.OpenTibiaId], 1) );

                        return Promise.Completed;
                    } );

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}