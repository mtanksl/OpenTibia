﻿using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PartyTrumpetHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly Dictionary<ushort, ushort> partyTrumpets;

        public PartyTrumpetHandler()
        {
            partyTrumpets = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.partyTrumpets");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (partyTrumpets.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.PartyAnimal, 200, "Party Animal") ).Then( () =>
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.GreenNotes) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ShowTextCommand(command.Player, MessageMode.MonsterSay, "TOOOOOOT!") );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
                } );
            }

            return next();
        }
    }
}