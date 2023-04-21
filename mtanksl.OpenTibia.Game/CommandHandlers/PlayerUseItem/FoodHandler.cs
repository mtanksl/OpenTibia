using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class FoodHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, (string Sound, int Regeneration) > foods = new Dictionary<ushort, (string Sound, int Regeneration) >()
        {
            // Banana
            { 2676, ("Yum.", 96) },

            // Bar of chocolate
            { 6574, ("Mmmm.", 60) },

            // Blueberry
            { 2677, ("Yum.", 12) },

            // Bread
            { 2689, ("Crunch.", 120) },

            // Brown Bread
            { 2691, ("Crunch.", 96) },

            // Brown Mushroom
            { 2789, ("Munch.", 264) },

            // Bulb of Garlic
            { 9114, ("Crunch.", 60) },

            // Cake
            { 6278, ("Mmmm.", 120) },

            // Candy
            { 6569, ("Mmmm.", 12) },

            // Candy Cane
            { 2688, ("Mmmm.", 24) },

            // Carrot
            { 2684, ("Crunch.", 60) },

            // Cheese
            { 8368, ("Smack.", 108) },

            // Cherry
            { 2679, ("Yum.", 12) },

            // Chocolate Cake
            { 8847, ("Yum.", 132) },

            // Coconut
            { 2678, ("Slurp.", 216) },

            // Coloured Egg (Blue)
            { 6543, ("Gulp.", 72) },

            // Coloured Egg (Green) 
            { 6544, ("Gulp.", 72) },

            // Coloured Egg (Purple)
            { 6545, ("Gulp.", 72) },

            // Coloured Egg (Red)
            { 6542, ("Gulp.", 72) },

            // Coloured Egg (Yellow)
            { 6541, ("Gulp.", 72) },

            // Cookie
            { 2687, ("Crunch.", 24) },

            // Corncob
            { 2686, ("Crunch.", 108) },

            // Cream Cake
            { 6394, ("Mmmm.", 180) },

            // Dark Mushroom
            { 2792, ("Munch.", 72) },
                        
            // Decorated Cake
            { 6279, ("Mmmm.", 180) },

            // Dragon Ham
            { 2672, ("Chomp.", 720) },

            // Egg
            { 2695, ("Gulp.", 72) },

            // Fish
            { 2667, ("Munch.", 144) },

            // Gingerbreadman
            { 6501, ("Mmmm", 240) },

            // Grapes
            { 2681, ("Yum.", 108) },

            // Green Mushroom
            { 2796, ("Munch.", 60) },

            // Ham
            { 2671, ("Munch.", 360) },

            // Ice Cream Cone (Blue-Barian)
            { 7377, ("Mmmm", 24) },

            // Ice Cream Cone (Chilly Cherry)
            { 7375, ("Mmmm", 24) },

            // Ice Cream Cone (Crispy Chocolate Chips)
            { 7372, ("Mmmm", 24) },

            // Ice Cream Cone (Mellow Melon)
            { 7376, ("Mmmm", 24) },

            // Ice Cream Cone (Sweet Strawberry)
            { 7374, ("Mmmm", 24) },

            // Ice Cream Cone (Velvet Vanilla)
            { 7373, ("Mmmm", 24) },

            // Jalapeño Pepper
            { 8844, ("Gulp", 12) },

             // Lemon
            { 8841, ("Urgh", 12) },

            // Mango
            { 5097, ("Yum.", 48) },

            // Marlin
            { 7963, ("Munch.", 720) },

             // Meat
            { 2666, ("Munch.", 180) },

            // Melon
            { 2682, ("Yum.", 240) },

            // Northern Pike
            { 2669, ("Munch.", 204) },

            // Onion
            { 8843, ("Crunch.", 60) },

            // Orange
            { 2675, ("Yum.", 156) },

            // Orange Mushroom
            { 2790, ("Munch.", 360) },

            // Party Cake
            { 6280, ("Mmmm.", 180) },

            // Peanut
            { 7910, ("Crunch.", 48) },

             // Pear
            { 2673, ("Yum.", 60) },

            // Plum
            { 8839, ("Yum.", 60) },

            // Potato
            { 8838, ("Gulp.", 120) },

            // Pumpkin
            { 2683, ("Munch.", 204) },

            // Rainbow Trout
            { 7158, ("Munch.", 300) },

            // Raspberry
            { 8840, ("Yum.", 12) },

            // Red Apple
            { 2674, ("Yum.", 72) },

            // Red Mushroom
            { 2788, ("Munch.", 48) },

            // Rice Ball
            { 11246, ("Yum.", 180) },

            // Roll
            { 2690, ("Crunch.", 36) },

            // Salmon
            { 2668, ("Mmmm.", 120) },

            // Shrimp
            { 2670, ("Gulp.", 48) },

            // Strawberry
            { 2680, ("Yum.", 24) },

            // Terramite Eggs
            { 11370, ("Urgh.", 36) },

            // Tomato
            { 2685, ("Munch.", 72) },

            // Tortoise Egg
            { 5678, ("Gulp.", 96) },

            // Valentine's Cake
            { 6393, ("Mmmm.", 144) },

            // Walnut
            { 7909, ("Crunch.", 48) },

             // White Mushroom
            { 2787, ("Munch.", 108) },

            // Wood Mushroom
            { 2791, ("Munch.", 108) },

            // Yummy Gummy Worm
            { 9005, ("Slurp.", 88) }
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            (string Sound, int Regeneration) food;

            if (foods.TryGetValue(command.Item.Metadata.OpenTibiaId, out food) )
            {
                CreatureConditionBehaviour creatureConditionBehaviour = Context.Server.Components.GetComponents<CreatureConditionBehaviour>(command.Player)
                    .Where(c => c.Condition.ConditionSpecialCondition == ConditionSpecialCondition.Regeneration)
                    .FirstOrDefault();

                if (creatureConditionBehaviour == null)
                {
                    return Context.AddCommand(new CreatureAddConditionCommand(command.Player, new RegenerationCondition(food.Regeneration) ) ).Then( () =>
                    {
                        return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) );

                    } ).Then(() =>
                    {
                        return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, food.Sound) );
                    } );
                }

                RegenerationCondition conditionRegeneration = (RegenerationCondition)creatureConditionBehaviour.Condition;

                if (conditionRegeneration.AddRegeneration(food.Regeneration) )
                {
                    return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then(() =>
                    {
                        return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, food.Sound) );
                    } );
                }

                Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouAreFull) );

                return Promise.Break;
            }

            return next();
        }
    }
}