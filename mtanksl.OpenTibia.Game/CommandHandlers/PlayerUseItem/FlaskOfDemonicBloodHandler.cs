using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FlaskOfDemonicBloodHandler : CommandHandler<PlayerUseItemCommand>
    {
        private static HashSet<ushort> flaskOfDemonicBloods = new HashSet<ushort>() { 6558 };

        private static ushort strongHealthPotion = 7588;

        private static ushort strongManaPotion = 7589;

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (flaskOfDemonicBloods.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.DemonicBarkeeper, 250, "Demonic Barkeeper") ).Then( () =>
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.RedSpark) );

                } ).Then( () =>
                {
                    if (Context.Server.Randomization.Take(1, 2) == 1)
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.Item, strongHealthPotion, 1) );
                    }
                    else
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.Item, strongManaPotion, 1) );
                    }
                } );
            }

            return next();
        }
    }
}