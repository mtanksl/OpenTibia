using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FoodHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, string> foods = new Dictionary<ushort, string>()
        {
            // Meat
            { 2666, "Munch." },

            // Banana
            { 2676, "Yum." },

            // Blueberry
            { 2677, "Yum." },

            // Cherry
            { 2679, "Yum." },

            // Grapes
            { 2681, "Yum." },

            // Lemon
            { 8841, "Urgh." },

            // Mango
            { 5097, "Yum." },

            // Melon
            { 2682, "Yum." },

            // Orange
            { 2675, "Yum." },

             // Pear
            { 2673, "Yum." },

            // Plum
            { 8839, "Yum." },

            // Raspberry
            { 8840, "Yum." },

            // Red Apple
            { 2674, "Yum." },

            // Strawberry
            { 2680, "Yum." },

            // Coconut
            { 2678, "Slurp." },

            // Pumpkin
            { 2683, "Munch." },

            // Bread
            { 2689, "Crunch." },

            // Cake
            { 6278, "Mmmm." },

            { 6279, "Mmmm." },

            { 6280, "Mmmm." },

            // Cookie
            { 2687, "Crunch." },

            // Bar of chocolate
            { 6574, "Mmmm." },

            // Chocolate Cake
            { 8847, "Yum." }
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            string message;

            if (foods.TryGetValue(command.Item.Metadata.OpenTibiaId, out message) )
            {
                return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                {
                    return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, message) );            
                } );
            }

            return next();
        }
    }
}