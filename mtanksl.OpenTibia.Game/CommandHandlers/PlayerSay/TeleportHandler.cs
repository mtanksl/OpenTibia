using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class TeleportHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            int count;

            if (command.Message.StartsWith("/a") && command.Message.Contains(" ") && int.TryParse(command.Message.Split(' ')[1], out count) && count > 0)
            {
                Tile toTile = Context.Server.Map.GetTile(command.Player.Tile.Position.Offset(command.Player.Direction, count) );

                if (toTile != null)
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) ).Then( () =>
                    {
                        return Context.AddCommand(new CreatureUpdateParentCommand(command.Player, toTile) );
                    } );
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
            }

            return next();
        }
    }
}