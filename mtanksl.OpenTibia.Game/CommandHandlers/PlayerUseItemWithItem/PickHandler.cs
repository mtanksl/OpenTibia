using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PickHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> picks;
        private readonly Dictionary<ushort, ushort> fragileIces;
        private readonly Dictionary<ushort, ushort> decay;

        public PickHandler()
        {
            picks = Context.Server.Values.GetUInt16HashSet("values.items.picks");
            fragileIces = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.fragileIces");
            decay = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.decay.fragileIces");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (picks.Contains(command.Item.Metadata.OpenTibiaId) && fragileIces.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.Puff) ).Then( () =>
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