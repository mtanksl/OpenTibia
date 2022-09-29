using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TilePressHandler : CommandHandler<CreatureMoveCommand>
    {
        private Dictionary<ushort, ushort> tiles = new Dictionary<ushort, ushort>()
        {
            { 416, 417 },
            { 426, 425 },
            { 446, 447 },
            { 3216, 3217 }
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(Context context, CreatureMoveCommand command)
        {
            Tile toTile = command.ToTile;

            if (toTile.Ground != null && tiles.TryGetValue(toTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) && !command.Data.ContainsKey("TilePressHandler") )
            {
                command.Data.Add("TilePressHandler", true);

                return true;
            }

            return false;
        }

        public override void Handle(Context context, CreatureMoveCommand command)
        {
            Tile toTile = command.ToTile;

            context.AddCommand(command);

            context.AddCommand(new ItemReplaceCommand(toTile.Ground, toOpenTibiaId, 1) );

            base.Handle(context, command);
        }
    }
}