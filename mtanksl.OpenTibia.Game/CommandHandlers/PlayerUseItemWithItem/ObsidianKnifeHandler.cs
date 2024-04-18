using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ObsidianKnifeHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private static HashSet<ushort> obsidianKnifes = new HashSet<ushort>() { 5908 };

        private static Dictionary<ushort, ushort> iceCubes = new Dictionary<ushort, ushort>()
        {
            { 7441, 7442 },
            { 7442, 7444 },
            { 7444, 7445 },
            { 7445, 7446 }
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (obsidianKnifes.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                ushort toOpenTibiaId;

                if (iceCubes.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    int value = Context.Server.Randomization.Take(1, 4);

                    if (value == 1)
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.Puff) ).Then( () =>
                        {
                            return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "The attempt at sculpting failed miserably.") );

                        } ).Then( () =>
                        {
                            return Context.AddCommand(new ItemDestroyCommand(command.ToItem) );
                        } );
                    }
                    else
                    {
                        if (toOpenTibiaId == 7446)
                        {
                            return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.IceSculptor, 1, "Ice Sculptor") ).Then( () =>
                            {
                                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.ColdAsIce, 10, "Cold as Ice") );

                            } ).Then( () =>
                            {
                                return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.Puff) );

                            } ).Then( () =>
                            {
                                return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "You shaped a perfect little mammoth from the ice cube.") );

                            } ).Then( () =>
                            {
                                return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );
                            } );
                        }
                        else
                        {
                            return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.Puff) ).Then(() =>
                            {
                                return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );
                            } );
                        }
                    }
                }
            }
            
            return next();
        }
    }
}