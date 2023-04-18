using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class DestroyMonsterOrNpcHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/r") && command.Player.Vocation == Vocation.Gamemaster)
            {
                Tile toTile = Context.Server.Map.GetTile(command.Player.Tile.Position.Offset(command.Player.Direction) );

                if (toTile != null)
                {
                    switch (toTile.TopCreature)
                    {
                        case Monster monster:

                            return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.RedShimmer) ).Then( () =>
                            {
                                return Context.AddCommand(new MonsterDestroyCommand(monster) );
                            } );

                        case Npc npc:

                            return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.RedShimmer) ).Then( () =>
                            {
                                return Context.AddCommand(new NpcDestroyCommand(npc) );
                            } );
                    }
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
            }

            return next();
        }
    }
}