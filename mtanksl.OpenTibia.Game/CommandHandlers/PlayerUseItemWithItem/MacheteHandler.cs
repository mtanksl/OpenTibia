using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class MacheteHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> machetes;
        private readonly Dictionary<ushort, ushort> jungleGrasses;

        public MacheteHandler()
        {
            machetes = Context.Server.Values.GetUInt16HashSet("values.items.machetes");
            jungleGrasses = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.jungleGrasses");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (machetes.Contains(command.Item.Metadata.OpenTibiaId) && jungleGrasses.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.NothingCanStopMe, 100, "Nothing Can Stop Me") ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );
                } );
            }

            return next();
        }
    }
}