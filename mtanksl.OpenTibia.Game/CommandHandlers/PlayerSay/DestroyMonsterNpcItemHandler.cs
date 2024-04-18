using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class DestroyMonsterNpcItemHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/r") )
            {
                Tile toTile = Context.Server.Map.GetTile(command.Player.Tile.Position.Offset(command.Player.Direction) );

                if (toTile != null)
                {
                    switch (toTile.TopCreature)
                    {
                        case Monster monster:
                        case Npc npc:

                            return Context.AddCommand(new CreatureDestroyCommand(toTile.TopCreature) ).Then( () =>
                            {
                                return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.RedShimmer) );
                            } );

                        default:

                            switch (toTile.TopItem)
                            {
                                case Item item:

                                    return Context.AddCommand(new ItemDestroyCommand(item) ).Then( () =>
                                    {
                                        return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.RedShimmer) );
                                    } );
                            }

                            break;
                    }
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );
            }

            return next();
        }
    }
}