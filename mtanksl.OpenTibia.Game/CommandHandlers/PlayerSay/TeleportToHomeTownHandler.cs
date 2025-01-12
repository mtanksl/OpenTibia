using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class TeleportToHomeTownHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/t ") )
            {
                string name = command.Message.Substring(3);

                Player observer = Context.Server.GameObjects.GetPlayerByName(name);

                if (observer != null && observer != command.Player)
                {
                    Tile toTile = observer.Town;

                    if (toTile != null)
                    {
                        Tile fromTile = observer.Tile;

                        return Context.AddCommand(new CreatureMoveCommand(observer, toTile) ).Then( () =>
                        {
                            return Context.AddCommand(new ShowMagicEffectCommand(fromTile.Position, MagicEffectType.Puff) );

                        } ).Then( () =>
                        {
                            return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
                        } );
                    }
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );
            }
            else if (command.Message.StartsWith("/t") )
            {
                Tile toTile = command.Player.Town;

                if (toTile != null)
                {
                    Tile fromTile = command.Player.Tile;

                    return Context.AddCommand(new CreatureMoveCommand(command.Player, toTile) ).Then( () =>
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(fromTile.Position, MagicEffectType.Puff) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
                    } );
                }
            }

            return next();
        }
    }
}