using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class HouseKickHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("alana sio \"") )
            {
                string name = command.Message.Substring(11).TrimEnd('\"');

                Player observer = Context.Server.GameObjects.GetPlayers()
                    .Where(p => p.Name == name)
                    .FirstOrDefault();

                if (observer != null)
                {
                    if (command.Player.Tile is HouseTile houseTile1 && observer.Tile is HouseTile houseTile2 && houseTile1.House == houseTile2.House && (houseTile1.House.IsOwner(command.Player.Name) || houseTile1.House.IsSubOwner(command.Player.Name) || observer == command.Player) )
                    {
                        Tile toTile = Context.Server.Map.GetTile(houseTile1.House.Entry);

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
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );
            }

            return next();
        }
    }
}