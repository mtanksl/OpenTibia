using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FireBugHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private static HashSet<ushort> fireBugs = new HashSet<ushort>() { 5468 };

        private static Dictionary<ushort, ushort> sugarCanes = new Dictionary<ushort, ushort>()
        {
            { 5466, 5465 }
        };

        private static Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>()
        {
            { 5465, 5471 },
            { 5471, 5466 }
        };

        private static Dictionary<ushort, ushort> emptyCoalBasins = new Dictionary<ushort, ushort>()
        {
            { 1485, 1483 }
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (fireBugs.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (sugarCanes.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    if (Context.Server.Randomization.HasProbability(1.0 / 10) )
                    {
                        return Context.AddCommand(new ItemDestroyCommand(command.Item) ).Then( () =>
                        {
                            return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "Ouch!") );

                        } ).Then( () =>
                        {
                            return Context.AddCommand(new CreatureAttackCreatureCommand(null, command.Player, 
                            
                                new SimpleAttack(null, MagicEffectType.ExplosionDamage, AnimatedTextColor.Orange, 5, 5) ) );
                        } );
                    }
                    else
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.FirePlume) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );

                        } ).Then( (item) =>
                        {
                            _ = Context.AddCommand(new ItemDecayTransformCommand(item, TimeSpan.FromSeconds(10), decay[item.Metadata.OpenTibiaId], 1) ).Then( (item2) =>
                            {
                                _ = Context.AddCommand(new ItemDecayTransformCommand(item2, TimeSpan.FromSeconds(10), decay[item2.Metadata.OpenTibiaId], 1) );

                                return Promise.Completed;
                            } );

                            return Promise.Completed;
                        } );
                    }
                }
                else if (emptyCoalBasins.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                { 
                    if (Context.Server.Randomization.HasProbability(1.0 / 10) )
                    {
                        return Context.AddCommand(new ItemDestroyCommand(command.Item) ).Then( () =>
                        {
                            return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "Ouch!") );

                        } ).Then( () =>
                        {
                            return Context.AddCommand(new CreatureAttackCreatureCommand(null, command.Player, 
                            
                                new SimpleAttack(null, MagicEffectType.ExplosionDamage, AnimatedTextColor.Orange, 5, 5) ) );
                        } );
                    }
                    else
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.FirePlume) ).Then( () =>
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