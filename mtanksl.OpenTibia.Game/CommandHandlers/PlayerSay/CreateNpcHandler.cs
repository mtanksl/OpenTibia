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
            if (command.Message.StartsWith("/n ") )
            {
                string name = command.Message.Substring(3);

                Tile toTile = Context.Server.Map.GetTile(command.Player.Tile.Position.Offset(command.Player.Direction) );

                if (toTile != null)
                {
                    return Context.AddCommand(new TileCreateNpcCommand(toTile, name) ).Then( (npc) =>
                    {
                        if (npc != null)
                        {
                            return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.BlueShimmer) );
                        }

                        return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );
                    } );
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );
            }

            return next();
        }
    }
}