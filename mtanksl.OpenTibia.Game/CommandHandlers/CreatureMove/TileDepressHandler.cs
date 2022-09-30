using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TileDepressHandler : CommandHandler<CreatureMoveCommand>
    {
        private Dictionary<ushort, ushort> tiles = new Dictionary<ushort, ushort>()
        {
            { 417, 416 },
            { 425, 426 },
            { 447, 446 },
            { 3217, 3216 }
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(Context context, CreatureMoveCommand command)
        {
            if (command.FromTile.Ground != null && tiles.TryGetValue(command.FromTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) && !command.Data.ContainsKey("TileDepressHandler") )
            {
                command.Data.Add("TileDepressHandler", true);

                return true;
            }

            return false;
        }

        public override void Handle(Context context, CreatureMoveCommand command)
        {
            context.AddCommand(command, ctx =>
            {
                ctx.AddCommand(new ItemReplaceCommand(command.FromTile.Ground, toOpenTibiaId, 1) );

                OnComplete(ctx);
            } );
        }
    }
}