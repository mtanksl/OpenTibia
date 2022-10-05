﻿using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TileDepressHandler : CommandHandler<CreatureUpdateParentCommand>
    {
        private Dictionary<ushort, ushort> tiles = new Dictionary<ushort, ushort>()
        {
            { 417, 416 },
            { 425, 426 },
            { 447, 446 },
            { 3217, 3216 }
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(Context context, CreatureUpdateParentCommand command)
        {
            if (command.Creature.Tile.Ground != null && tiles.TryGetValue(command.Creature.Tile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) && !command.Data.ContainsKey("TileDepressHandler") )
            {
                command.Data.Add("TileDepressHandler", true);

                return true;
            }

            return false;
        }

        public override void Handle(Context context, CreatureUpdateParentCommand command)
        {
            Tile fromTile = command.Creature.Tile;

            context.AddCommand(command).Then(ctx =>
            {
                return ctx.AddCommand(new ItemTransformCommand(fromTile.Ground, toOpenTibiaId, 1) );

            } ).Then( (ctx, item) =>
            {
                OnComplete(ctx);
            } );
        }
    }
}