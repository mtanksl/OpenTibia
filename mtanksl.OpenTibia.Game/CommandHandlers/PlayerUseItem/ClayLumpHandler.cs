using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ClayLumpHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> clayLumps;
        private readonly ushort roughClayStatue;
        private readonly ushort clayStatue;
        private readonly ushort prettyClayStatue;

        public ClayLumpHandler()
        {
            clayLumps = Context.Server.Values.GetUInt16HashSet("values.items.clayLumps");
            roughClayStatue = Context.Server.Values.GetUInt16("values.items.roughClayStatue");
            clayStatue = Context.Server.Values.GetUInt16("values.items.clayStatue");
            prettyClayStatue = Context.Server.Values.GetUInt16("values.items.prettyClayStatue");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (clayLumps.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                int value = Context.Server.Randomization.Take(1, 4);

                if (value == 1)
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.Puff) ).Then( () =>
                    {
                        return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "Aw man. That did not work out too well.") );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new ItemDestroyCommand(command.Item) );
                    } );
                }
                else if (value == 2)
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.Puff) ).Then(() =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.Item, roughClayStatue, 1) );
                    } );
                }
                else if (value == 3)
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.Puff) ).Then(() =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.Item, clayStatue, 1) );
                    } );
                }
                else
                {
                    return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.ClayFighter, 1, "Clay Fighter") ).Then( () =>
                    {
                        return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.ClayToFame, 5, "Clay to Fame") );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.Puff) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.Item, prettyClayStatue, 1) );
                    } );
                }
            }

            return next();
        }
    }
}