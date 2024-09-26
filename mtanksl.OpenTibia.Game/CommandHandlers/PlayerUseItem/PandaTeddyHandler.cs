using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PandaTeddyHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> pandaTeddies;

        public PandaTeddyHandler()
        {
            pandaTeddies = LuaScope.GetInt16HashSet(Context.Server.Values.GetValue("values.items.pandaTeddies") );
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (pandaTeddies.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.INeedAHugPandaTeddy, new[] { AchievementConstants.INeedAHugPandaTeddy, AchievementConstants.INeedAHugStuffedDragon, AchievementConstants.INeedAHugBabySealDoll, AchievementConstants.INeedAHugSantaDoll }, "I Need a Hug") ).Then( () =>
                {
                    return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "Hug me!") );
                } );                
            }

            return next();
        }
    }
}