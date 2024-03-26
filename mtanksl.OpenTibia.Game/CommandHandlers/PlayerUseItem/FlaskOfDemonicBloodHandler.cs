using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FlaskOfDemonicBloodHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> flaskOfDemonicBloods = new HashSet<ushort>() { 6558 };

        private ushort strongHealthPotion = 7588;

        private ushort strongManaPotion = 7589;

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (flaskOfDemonicBloods.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.DemonicBarkeeper, 250, "Demonic Barkeeper") ).Then( (Func<Promise>)(() =>
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

                        case LockerCollection safe:

                            position = safe.Player.Tile.Position;

                            break;
                    }

                    if (position != null)
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType.RedSpark) );
                    }

                    return Promise.Completed;

                }) ).Then( () =>
                {
                    if (Context.Server.Randomization.Take(1, 2) == 1)
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.Item, strongHealthPotion, 1) );
                    }
                    
                    return Context.AddCommand(new ItemTransformCommand(command.Item, strongManaPotion, 1));
                } );
            }

            return next();
        }
    }
}