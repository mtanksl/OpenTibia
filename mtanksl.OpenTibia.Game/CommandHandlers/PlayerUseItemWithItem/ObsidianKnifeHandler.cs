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
        private readonly ushort iceMammoth;
        private readonly HashSet<ushort> marbleRocks;
        private readonly ushort roughMarbleStatue;
        private readonly ushort marbleStatue;
        private readonly ushort beautifulMarbleStatue;
        private readonly Dictionary<ushort, ushort> corpses;

        public ObsidianKnifeHandler()
        {
            obsidianKnifes = Context.Server.Values.GetUInt16HashSet("values.items.obsidianKnifes");
            iceCubes = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.iceCubes");
            iceMammoth = Context.Server.Values.GetUInt16("values.items.iceMammoth");
            marbleRocks = Context.Server.Values.GetUInt16HashSet("values.items.marbleRocks");
            roughMarbleStatue = Context.Server.Values.GetUInt16("values.items.roughMarbleStatue");
            marbleStatue = Context.Server.Values.GetUInt16("values.items.marbleStatue");
            beautifulMarbleStatue = Context.Server.Values.GetUInt16("values.items.beautifulMarbleStatue");
            corpses = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.corpses");
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
                        if (toOpenTibiaId == iceMammoth)
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
                else if (marbleRocks.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    int value = Context.Server.Randomization.Take(1, 4);

                    if (value == 1)
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.Puff) ).Then( () =>
                        {
                            return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "Aw man. That did not work out too well.") );

                        } ).Then( () =>
                        {
                            return Context.AddCommand(new ItemDestroyCommand(command.ToItem) );
                        } );
                    }
                    else if (value == 2)
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.Puff) ).Then(() =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, roughMarbleStatue, 1) );
                        } );
                    }
                    else if (value == 3)
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.Puff) ).Then(() =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, marbleStatue, 1) );
                        } );
                    }
                    else
                    {
                        return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.Marblelous, 1, "Marblelous") ).Then( () =>
                        {
                            return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.MarbleMadness, 5, "Marble Madness") );

                        } ).Then( () =>
                        {
                            return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.Puff) );

                        } ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, beautifulMarbleStatue, 1) );
                        } );
                    }
                }
                else if (corpses.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    //TODO
                }
            }
            
            return next();
        }
    }
}