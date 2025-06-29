using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class WrapItemTransformHandler : CommandHandler<PlayerWrapItemCommand>
    {
        private readonly Dictionary<ushort, ushort> transformations;

        public WrapItemTransformHandler()
        {
            transformations = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.wrap");
        }

        public override Promise Handle(Func<Promise> next, PlayerWrapItemCommand command)
        {
            ushort toOpenTibiaId;

            if (transformations.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
            }

            return next();
        }
    }
}