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
        private readonly Dictionary<ushort, ushort> corpseBehemoths;
        private readonly Dictionary<ushort, ushort> corpseBonebeasts;
        private readonly Dictionary<ushort, ushort> corpseDragons;
        private readonly Dictionary<ushort, ushort> corpseDragonLords;
        private readonly Dictionary<ushort, ushort> corpseLizards;
        private readonly Dictionary<ushort, ushort> corpseMinotaurs;
        private readonly ushort perfectBehemothFang;
        private readonly ushort hardenedBone;
        private readonly ushort greenDragonLeather;
        private readonly ushort redDragonLeather;
        private readonly ushort lizardLeather;
        private readonly ushort minotaurLeather;

        public ObsidianKnifeHandler()
        {
            obsidianKnifes = Context.Server.Values.GetUInt16HashSet("values.items.obsidianKnifes");
            iceCubes = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.iceCubes");
            iceMammoth = Context.Server.Values.GetUInt16("values.items.iceMammoth");
            marbleRocks = Context.Server.Values.GetUInt16HashSet("values.items.marbleRocks");
            roughMarbleStatue = Context.Server.Values.GetUInt16("values.items.roughMarbleStatue");
            marbleStatue = Context.Server.Values.GetUInt16("values.items.marbleStatue");
            beautifulMarbleStatue = Context.Server.Values.GetUInt16("values.items.beautifulMarbleStatue");
            corpseBehemoths = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.corpseBehemoths");
            corpseBonebeasts = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.corpseBonebeasts");
            corpseDragons = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.corpseDragons");
            corpseDragonLords = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.corpseDragonLords");
            corpseLizards = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.corpseLizards");
            corpseMinotaurs = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.corpseMinotaurs");
            perfectBehemothFang = Context.Server.Values.GetUInt16("values.items.perfectBehemothFang");
            hardenedBone = Context.Server.Values.GetUInt16("values.items.hardenedBone");
            greenDragonLeather = Context.Server.Values.GetUInt16("values.items.greenDragonLeather");
            redDragonLeather = Context.Server.Values.GetUInt16("values.items.redDragonLeather");
            lizardLeather = Context.Server.Values.GetUInt16("values.items.lizardLeather");
            minotaurLeather = Context.Server.Values.GetUInt16("values.items.minotaurLeather");
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
                else if (corpseBehemoths.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    if (Context.Server.Randomization.HasProbability(10 / 100.0) )
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.GreenShimmer) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );

                        } ).Then( (item) =>
                        {
                            return Context.AddCommand(new PlayerCreateItemCommand(command.Player, perfectBehemothFang, 1) );
                        } );
                    }
                    else
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.YellowSpark) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );
                        } );
                    }
                }
                else if (corpseBonebeasts.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    if (Context.Server.Randomization.HasProbability(5 / 100.0) )
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.GreenShimmer) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );

                        } ).Then( (item) =>
                        {
                            return Context.AddCommand(new PlayerCreateItemCommand(command.Player, hardenedBone, 1) );
                        } );
                    }
                    else
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.YellowSpark) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );
                        } );
                    }
                }
                else if (corpseDragons.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    if (Context.Server.Randomization.HasProbability(5 / 100.0) )
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.GreenShimmer) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );

                        } ).Then( (item) =>
                        {
                            return Context.AddCommand(new PlayerCreateItemCommand(command.Player, greenDragonLeather, 1) );
                        } );
                    }
                    else
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.YellowSpark) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );
                        } );
                    }
                }
                else if (corpseDragonLords.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    if (Context.Server.Randomization.HasProbability(10 / 100.0) )
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.GreenShimmer) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );

                        } ).Then( (item) =>
                        {
                            return Context.AddCommand(new PlayerCreateItemCommand(command.Player, redDragonLeather, 1) );
                        } );
                    }
                    else
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.YellowSpark) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );
                        } );
                    }
                }
                else if (corpseLizards.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    if (Context.Server.Randomization.HasProbability(10 / 100.0) )
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.GreenShimmer) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );

                        } ).Then( (item) =>
                        {
                            return Context.AddCommand(new PlayerCreateItemCommand(command.Player, lizardLeather, 1) );
                        } );
                    }
                    else
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.YellowSpark) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );
                        } );
                    }
                }
                else if (corpseMinotaurs.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    if (Context.Server.Randomization.HasProbability(10 / 100.0) )
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.GreenShimmer) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );

                        } ).Then( (item) =>
                        {
                            return Context.AddCommand(new PlayerCreateItemCommand(command.Player, minotaurLeather, 1) );
                        } );
                    }
                    else
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.YellowSpark) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );
                        } );
                    }
                }
            }
            
            return next();
        }
    }
}