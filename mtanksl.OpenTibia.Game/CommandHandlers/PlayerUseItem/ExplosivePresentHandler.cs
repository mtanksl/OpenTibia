using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ExplosivePresentHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> explosivePresent = new HashSet<ushort>() { 8110 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (explosivePresent.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.JokesOnYou, 1, "Jokes On You") ).Then( () =>
                {
                    Position position = null;

                    switch (command.Item.Root() )
                    {
                        case Tile tile:

                            position = tile.Position;

                            break;

                        case Inventory inventory:

                            position = inventory.Player.Tile.Position;

                            break;

                        case Safe safe:

                            position = safe.Player.Tile.Position;

                            break;
                    }

                    if (position != null)
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType.ExplosionDamage) );
                    }

                    return Promise.Completed;

                } ).Then( () =>
                {
                    return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "KABOOOOOOOOOOM!") );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ItemDestroyCommand(command.Item) );
                } );
            }

            return next();
        }
    }
}