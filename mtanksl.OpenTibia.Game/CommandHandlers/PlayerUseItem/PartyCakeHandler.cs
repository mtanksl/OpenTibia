using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PartyCakeHandler : CommandHandler<PlayerUseItemCommand>
    {
        private static HashSet<ushort> partyCakes = new HashSet<ushort>() { 6280 };

        private static ushort decoratedCake = 6279;

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