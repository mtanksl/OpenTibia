using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class TeleportToWaypointHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Context context, ContextPromiseDelegate next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/w") )
            {
                int startIndex = command.Message.IndexOf(' ');

                if (startIndex != -1)
                {
                    string name = command.Message.Substring(startIndex + 1);

                    Waypoint waypoint = context.Server.Map.GetWaypoint(name);

                    if (waypoint != null)
                    {
                        Tile toTile = context.Server.Map.GetTile(waypoint.Position);

                        if (toTile != null)
                        {
                            return context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) ).Then(ctx =>
                            {
                                return ctx.AddCommand(new CreatureUpdateParentCommand(command.Player, toTile) );
                            } );
                        }
                    }

                    return context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
                }
            }

            return next(context);
        }
    }
}