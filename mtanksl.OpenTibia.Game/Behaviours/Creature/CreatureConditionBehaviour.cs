using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Components
{
    public class CreatureConditionBehaviour : Behaviour
    {
        public CreatureConditionBehaviour(Condition condition)
        {
            this.condition = condition;
        }

        private Condition condition;

        public Condition Condition
        {
            get
            {
                return condition;
            }
        }

        private Creature target;

        public override void Start(Server server)
        {
            target = (Creature)GameObject;

            if ( !target.HasSpecialCondition( (SpecialCondition)condition.ConditionSpecialCondition) )
            {
                target.AddSpecialCondition( (SpecialCondition)condition.ConditionSpecialCondition);

                if (target is Player player)
                {
                    Context.Current.AddPacket(player.Client.Connection, new SetSpecialConditionOutgoingPacket(target.SpecialConditions) );
                }
            }

            condition.Start(target).Then( () =>
            {
                server.Components.RemoveComponent(GameObject, this);

            } ).Catch( (ex) =>
            {
                if (ex is PromiseCanceledException)
                {
                                
                }
                else
                {
                    server.Logger.WriteLine(ex.ToString(), LogLevel.Error);
                }
            } );
        }

        public override void Stop(Server server)
        {
            target.RemoveSpecialCondition( (SpecialCondition)condition.ConditionSpecialCondition);

            if (target is Player player)
            {
                Context.Current.AddPacket(player.Client.Connection, new SetSpecialConditionOutgoingPacket(target.SpecialConditions) );
            }

            condition.Stop(target).Catch( (ex) =>
            {
                if (ex is PromiseCanceledException)
                {
                                
                }
                else
                {
                    server.Logger.WriteLine(ex.ToString(), LogLevel.Error);
                }
            } );
        }
    }
}