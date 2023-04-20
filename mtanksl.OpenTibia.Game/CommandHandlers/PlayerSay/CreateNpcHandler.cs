using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class CreateNpcHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/n ") && command.Player.Vocation == Vocation.Gamemaster)
            {
                string name = command.Message.Substring(3);

                Tile fromTile = command.Player.Tile;

                Tile toTile = Context.Server.Map.GetTile(fromTile.Position.Offset(command.Player.Direction) );

                if (toTile != null)
                {
                    return Context.AddCommand(new TileCreateNpcCommand(toTile, name) ).Then( (npc) =>
                    {
                        if (npc != null)
                        {
                            return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.BlueShimmer) );
                        }

                        return Context.AddCommand(new ShowMagicEffectCommand(fromTile.Position, MagicEffectType.Puff) );
                    } );
                }

                return Context.AddCommand(new ShowMagicEffectCommand(fromTile.Position, MagicEffectType.Puff) );
            }

            return next();
        }
    }
}