using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class DestroyMonsterOrNpcHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(ContextPromiseDelegate next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/r") )
            {
                Tile toTile = context.Server.Map.GetTile(command.Player.Tile.Position.Offset(command.Player.Direction) );

                if (toTile != null)
                {
                    switch (toTile.TopCreature)
                    {
                        case Monster monster:

                            return context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.RedShimmer) ).Then(ctx =>
                            {
                                return ctx.AddCommand(new MonsterDestroyCommand(monster) );
                            } );

                        case Npc npc:

                            return context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.RedShimmer) ).Then(ctx =>
                            {
                                return ctx.AddCommand(new NpcDestroyCommand(npc) );
                            } );
                    }
                }

                return context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
            }

            return next(context);
        }
    }
}