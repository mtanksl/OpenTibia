using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class OutfitCondition : CreatureConditionBehaviour
    {
        public OutfitCondition(Outfit outfit, TimeSpan duration) : base(ConditionSpecialCondition.Outfit)
        {
            Outfit = outfit;

            Duration = duration;
        }

        public Outfit Outfit { get; set; }

        public TimeSpan Duration { get; set; }

        private string key = Guid.NewGuid().ToString();

        public override async void Start()
        {
            base.Start();

            Creature creature = (Creature)GameObject;

            try
            {
                await Context.AddCommand(new CreatureUpdateOutfitCommand(creature, creature.BaseOutfit, Outfit));

                await Promise.Delay(key, Duration);

                Context.Server.GameObjectComponents.RemoveComponent(creature, this);
            }
            catch (PromiseCanceledException) { }
        }

        public override async void Stop()
        {
            base.Stop();

            Creature creature = (Creature)GameObject;

            await Context.AddCommand(new CreatureUpdateOutfitCommand(creature, creature.BaseOutfit, creature.BaseOutfit) );

            Context.Server.CancelQueueForExecution(key);
        }
    }
}