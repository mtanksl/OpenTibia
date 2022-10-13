using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class DestroyMonsterOrNpcHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/r") )
            {
                Tile toTile = context.Server.Map.GetTile(command.Player.Tile.Position.Offset(command.Player.Direction) );

                if (toTile != null)
                {
                    switch (toTile.TopCreature)
                    {
                        case Monster monster:

                            return context.AddCommand(new MonsterDestroyCommand(monster) ).Then(ctx =>
                            {
                                return ctx.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.RedShimmer) );
                            } );

                        case Npc npc:

                            return context.AddCommand(new NpcDestroyCommand(npc) ).Then(ctx =>
                            {
                                return ctx.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.RedShimmer) );
                            } );
                    }
                }

                return context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
            }

            return next(context);
        }
    }
}