using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BabySealDollHandler : CommandHandler<PlayerUseItemCommand>
    {
        private static Dictionary<ushort, ushort> babySealDolls = new Dictionary<ushort, ushort>() 
        {
            { 7183, 7184 }
        };

        private static Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>() 
        {
            { 7184, 7183 }
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (babySealDolls.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.INeedAHugBabySealDoll, new[] { AchievementConstants.INeedAHugPandaTeddy, AchievementConstants.INeedAHugStuffedDragon, AchievementConstants.INeedAHugBabySealDoll, AchievementConstants.INeedAHugSantaDoll }, "I Need a Hug") ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );

                } ).Then( (item) =>
                {
                    _ = Context.AddCommand(new ItemDecayTransformCommand(item, TimeSpan.FromSeconds(3), decay[item.Metadata.OpenTibiaId], 1) );

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}