using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.Commands
{
    public class ConditionLight : Condition
    {
        private DelayBehaviour delayBehaviour;

        public ConditionLight(Light light, int durationInMilliseconds) : base(ConditionSpecialCondition.Light)
        {
            Light = light;

            DurationInMilliseconds = durationInMilliseconds;
        }

        public Light Light { get; set; }

        public int DurationInMilliseconds { get; set; }

        public override Promise Update(Creature target)
        {
            return Context.Current.AddCommand(new CreatureUpdateLightCommand(target, Light) ).Then( () =>
            {
                delayBehaviour = Context.Current.Server.Components.AddComponent(target, new DelayBehaviour(DurationInMilliseconds) );

                return delayBehaviour.Promise;

            } ).Then( () =>
            {
                return Context.Current.AddCommand(new CreatureUpdateLightCommand(target, Light.None) );
            } );
        }

        public override void Stop(Server server)
        {
            if (delayBehaviour != null)
            {
                delayBehaviour.Stop(server);
            }
        }
    }
}