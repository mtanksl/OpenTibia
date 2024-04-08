using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class MacheteHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private static HashSet<ushort> machetes = new HashSet<ushort>() { 2420, 10513, 10515, 10511 };

        private static Dictionary<ushort, ushort> jungleGrass = new Dictionary<ushort, ushort>()
        {
            { 2782, 2781 },
            { 3985, 3984 }
        };

        private static Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>()
        {
            { 2781, 2782 },
            { 3984, 3985 }
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (machetes.Contains(command.Item.Metadata.OpenTibiaId) && jungleGrass.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.NothingCanStopMe, 100, "Nothing Can Stop Me") ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );

                } ).Then( (item) =>
                {
                    _ = Context.AddCommand(new ItemDecayTransformCommand(item, TimeSpan.FromSeconds(10), decay[item.Metadata.OpenTibiaId], 1) );

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}