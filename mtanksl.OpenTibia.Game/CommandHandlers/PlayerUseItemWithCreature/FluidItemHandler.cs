using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class FluidItemHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            if (command.Item is FluidItem fromItem && command.ToCreature is Player toPlayer)
            {
                if (fromItem.FluidType == FluidType.Empty)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ItIsEmpty) );

                    return Promise.Break;
                }

                if (fromItem.FluidType == FluidType.Beer || fromItem.FluidType == FluidType.Wine || fromItem.FluidType == FluidType.Rum)
                {
                    return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Empty) ).Then( () =>
                    {
                        return Context.AddCommand(new ShowTextCommand(toPlayer, TalkType.MonsterSay, "Aah...") );
                    
                    } ).Then( () =>
                    {
                        return Context.AddCommand(new CreatureAddConditionCommand(toPlayer, new DrunkCondition(TimeSpan.FromMinutes(2) ) ) );
                    } );
                }

                if (fromItem.FluidType == FluidType.Slime)
                {
                    return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Empty) ).Then( () =>
                    {
                        return Context.AddCommand(new CreatureAttackCreatureCommand(null, toPlayer, 

                            new DamageAttack(null, null, DamageType.Earth, 5, 5),
                                                                                                                         
                            new DamageCondition(SpecialCondition.Poisoned, null, DamageType.Earth, new[] { 5, 5, 5, 5, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, TimeSpan.FromSeconds(4) ) ) );
                    
                    } ).Then( () =>
                    {
                        return Context.AddCommand(new ShowTextCommand(toPlayer, TalkType.MonsterSay, "Urgh!") );
                    } );
                }

                if (fromItem.FluidType == FluidType.Lifefluid)
                {
                    return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Empty) ).Then( () =>
                    {
                        return Context.AddCommand(new CreatureUpdateHealthCommand(toPlayer, toPlayer.Health + 60) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(toPlayer, MagicEffectType.RedShimmer) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new ShowTextCommand(toPlayer, TalkType.MonsterSay, "Aaaah...") );
                    } );
                }

                if (fromItem.FluidType == FluidType.Manafluid)
                {
                    return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Empty) ).Then( () =>
                    {
                        return Context.AddCommand(new PlayerUpdateManaCommand(toPlayer, toPlayer.Mana + Context.Server.Randomization.Take(50, 150) ) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(toPlayer, MagicEffectType.BlueShimmer) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new ShowTextCommand(toPlayer, TalkType.MonsterSay, "Aaaah...") );
                    } );
                }

                if (fromItem.FluidType == FluidType.Urine)
                {
                    return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Empty) ).Then( () =>
                    {
                        return Context.AddCommand(new ShowTextCommand(toPlayer, TalkType.MonsterSay, "Urgh!") );
                    } );
                }

                return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Empty) ).Then( () =>
                {
                    return Context.AddCommand(new ShowTextCommand(toPlayer, TalkType.MonsterSay, "Gulp.") );
                } ); 
            }

            return next();
        }
    }
}