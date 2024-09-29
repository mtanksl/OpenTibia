using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FlaskOfDemonicBloodHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> flaskOfDemonicBloods;
        private readonly ushort strongHealthPotion;
        private readonly ushort strongManaPotion;

        public FlaskOfDemonicBloodHandler()
        {
            flaskOfDemonicBloods = Context.Server.Values.GetUInt16HashSet("values.items.flaskOfDemonicBloods");
            strongHealthPotion = Context.Server.Values.GetUInt16("values.items.strongHealthPotion");
            strongManaPotion = Context.Server.Values.GetUInt16("values.items.strongManaPotion");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (flaskOfDemonicBloods.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.DemonicBarkeeper, 250, "Demonic Barkeeper") ).Then( () =>
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.RedSpark) );

                } ).Then( () =>
                {
                    if (Context.Server.Randomization.HasProbability(1.0 / 2) )
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