using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ObsidianKnifeHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> obsidianKnifes;
        private readonly Dictionary<ushort, ushort> iceCubes;

        public ObsidianKnifeHandler()
        {
            obsidianKnifes = Context.Server.Values.GetUInt16HashSet("values.items.obsidianKnifes");
            iceCubes = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.iceCubes");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (obsidianKnifes.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                ushort toOpenTibiaId;

                if (iceCubes.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    if (Context.Server.Randomization.HasProbability(1.0 / 4) )
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