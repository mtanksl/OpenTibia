using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PartyCakeHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> partyCakes;
        private readonly ushort decoratedCake;

        public PartyCakeHandler()
        {
            partyCakes = LuaScope.GetInt16HashSet(Context.Server.Values.GetValue("values.items.partyCakes") );
            decoratedCake = LuaScope.GetInt16(Context.Server.Values.GetValue("values.items.decoratedCake") );
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (partyCakes.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.MakeAWish, 5, "Make a Wish") ).Then( () =>
                {
                    return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, command.Player.Name + " blew out the candle.") );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.Item, decoratedCake, 1) );
                } );        
            }

            return next();
        }
    }
}