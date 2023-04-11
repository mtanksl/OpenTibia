using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class TeleportToPlayerHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/goto") )
            {
                int startIndex = command.Message.IndexOf(' ');

                if (startIndex != -1)
                {
                    string name = command.Message.Substring(startIndex + 1);

                    Player player = Context.Server.GameObjects.GetPlayers()
                        .Where(p => p.Name == name)
                        .FirstOrDefault();

                    if (player != null && player != command.Player)
                    {
                        Tile toTile = player.Tile;

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
            }

            return next();
        }
    }
}