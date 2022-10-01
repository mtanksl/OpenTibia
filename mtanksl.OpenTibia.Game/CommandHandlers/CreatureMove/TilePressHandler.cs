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
            if (command.ToTile.Ground != null && tiles.TryGetValue(command.ToTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) && !command.Data.ContainsKey("TilePressHandler") )
            {
                command.Data.Add("TilePressHandler", true);

                return true;
            }

            return false;
        }

        public override void Handle(Context context, CreatureMoveCommand command)
        {
            context.AddCommand(command).Then(ctx =>
            {
                return ctx.AddCommand(new ItemReplaceCommand(command.ToTile.Ground, toOpenTibiaId, 1) );

            } ).Then( (ctx, item) =>
            {
                OnComplete(ctx);
            } );
        }
    }
}