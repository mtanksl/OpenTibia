using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class TeleportToTownHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/t ") )
            {
                string name = command.Message.Substring(3);

                Town town = Context.Server.Map.GetTown(name);

                if (town != null)
                {
                    Tile toTile = Context.Server.Map.GetTile(town.Position);

                    if (toTile != null)
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) ).Then( () =>
                        {
                            return Context.AddCommand(new CreatureUpdateParentCommand(command.Player, toTile) );
                        } );
                    }
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
            }

            return next();
        }
    }
}