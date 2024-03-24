using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class DisplayProjectileTypeHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            int id;

            if (command.Message.StartsWith("/pe ") && int.TryParse(command.Message.Substring(4), out id) && id >= 1 && id <= 42)
            {
                Tile fromTile = command.Player.Tile;

                Offset[] area = new Offset[]
                {
                    new Offset(-2, -2), new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2), new Offset(2, -2),
                    new Offset(-2, -1),                                                           new Offset(2, -1),
                    new Offset(-2, 0),                                                            new Offset(2, 0),
                    new Offset(-2, 1),                                                            new Offset(2, 1),
                    new Offset(-2, 2),  new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2),  new Offset(2, 2)
                };

                List<Promise> promises = new List<Promise>();

                foreach (var offset in area)
                {
                    promises.Add(Context.AddCommand(new ShowProjectileCommand(fromTile.Position, fromTile.Position.Offset(offset), (ProjectileType)id) ) );
                }

                return Promise.WhenAll(promises.ToArray() );
            }

            return next();
        }
    }
}