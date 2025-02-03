using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CrowbarHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> crowbars;
        private readonly Dictionary<ushort, ushort> trashes;

        public CrowbarHandler()
        {
            crowbars = Context.Server.Values.GetUInt16HashSet("values.items.crowbars");
            trashes = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.trashes");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (crowbars.Contains(command.Item.Metadata.OpenTibiaId) && trashes.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                if ( !( (command.ToItem is Container container && container.Count > 0) || command.ToItem.UniqueId > 0) )
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.Puff) ).Then( () =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );
                    } );
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.Puff) );
            }

            return next();
        }
    }
}