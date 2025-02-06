using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BlessedWoodenStakeHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> blessedWoodenStakes;
        private readonly Dictionary<ushort, ushort> demonDusts;
        private readonly Dictionary<ushort, ushort> vampireDusts;
        private readonly ushort demonDust;
        private readonly ushort vampireDust;

        public BlessedWoodenStakeHandler()
        {
            blessedWoodenStakes = Context.Server.Values.GetUInt16HashSet("values.items.blessedWoodenStakes");
            demonDusts = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.demonDusts");
            vampireDusts = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.vampireDusts");
            demonDust = Context.Server.Values.GetUInt16("values.items.demonDust");
            vampireDust = Context.Server.Values.GetUInt16("values.items.vampireDust");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (blessedWoodenStakes.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (demonDusts.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    if (Context.Server.Randomization.HasProbability(10 / 100.0) )
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.GreenShimmer) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );

                        } ).Then( (item) =>
                        {
                            return Context.AddCommand(new PlayerCreateItemCommand(command.Player, demonDust, 1) );
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
                else if (vampireDusts.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    if (Context.Server.Randomization.HasProbability(5 / 100.0) )
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.GreenShimmer) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );

                        } ).Then( (item) =>
                        {
                            return Context.AddCommand(new PlayerCreateItemCommand(command.Player, vampireDust, 1) );
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