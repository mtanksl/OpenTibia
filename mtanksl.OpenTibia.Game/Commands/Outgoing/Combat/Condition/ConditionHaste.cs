using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.Commands
{
    public class ConditionHaste : Condition
    {
        private DelayBehaviour delayBehaviour;

        public ConditionHaste(ushort speed, int durationInMilliseconds) : base(ConditionSpecialCondition.Haste)
        {
            Speed = speed;

            DurationInMilliseconds = durationInMilliseconds;
        }

        public ushort Speed { get; set; }

        public int DurationInMilliseconds { get; set; }

        public override Promise Update(Creature target)
        {
            return Context.Current.AddCommand(new CreatureUpdateSpeedCommand(target, target.BaseSpeed, Speed) ).Then( () =>
            {
                delayBehaviour = Context.Current.Server.Components.AddComponent(target, new DelayBehaviour(DurationInMilliseconds) );

                return delayBehaviour.Promise;

            } ).Then( () =>
            {
                return Context.Current.AddCommand(new CreatureUpdateSpeedCommand(target, target.BaseSpeed, target.BaseSpeed) );
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