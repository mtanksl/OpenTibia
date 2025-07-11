﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FireworksRocketHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> fireworksRockets;

        public FireworksRocketHandler()
        {
            fireworksRockets = Context.Server.Values.GetUInt16HashSet("values.items.fireworksRockets");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (fireworksRockets.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (command.Item.Parent is Tile)
                {
                    return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.FireworksInTheSky, 250, "Fireworks in the Sky") ).Then( () =>
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.FireworkBlue) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new ItemDestroyCommand(command.Item) );
                    } );
                }

                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.RocketInPocket, 3, "Rocket in Pocket") ).Then( () =>
                {
                    return Context.AddCommand(new ShowTextCommand(command.Player, MessageMode.MonsterSay, "Ouch! Rather place it on the ground next time.") );

                } ).Then( () =>
                {
                    return Context.AddCommand(new CreatureAttackCreatureCommand(null, command.Player, 
                        
                        new DamageAttack(null, MagicEffectType.ExplosionDamage, DamageType.Fire, 10, 10, false) ) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ItemDestroyCommand(command.Item) );
                } );
            }

            return next();
        }
    }
}