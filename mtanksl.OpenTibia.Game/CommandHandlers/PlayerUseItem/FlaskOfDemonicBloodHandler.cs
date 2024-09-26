using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
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
            flaskOfDemonicBloods = LuaScope.GetInt16HashSet(Context.Server.Values.GetValue("values.items.flaskOfDemonicBloods") );
            strongHealthPotion = LuaScope.GetInt16(Context.Server.Values.GetValue("values.items.strongHealthPotion") );
            strongManaPotion = LuaScope.GetInt16(Context.Server.Values.GetValue("values.items.strongManaPotion") );
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