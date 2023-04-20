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

        public override bool IsUnique
        {
            get
            {
                return false;
            }
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

            SpecialCondition specialCondition = SpecialCondition.None;

            switch (Condition.ConditionSpecialCondition)
            {
                case ConditionSpecialCondition.Poisoned:

                    specialCondition = SpecialCondition.Poisoned;

                    break;

                case ConditionSpecialCondition.Burning:

                    specialCondition = SpecialCondition.Burning;

                    break;

                case ConditionSpecialCondition.Electrified:

                    specialCondition = SpecialCondition.Electrified;

                    break;

                case ConditionSpecialCondition.Drunk:

                    specialCondition = SpecialCondition.Drunk;

                    break;

                case ConditionSpecialCondition.MagicShield:

                    specialCondition = SpecialCondition.MagicShield;

                    break;

                case ConditionSpecialCondition.Slowed:

                    specialCondition = SpecialCondition.Slowed;

                    break;

                case ConditionSpecialCondition.Haste:

                    specialCondition = SpecialCondition.Haste;

                    break;

                case ConditionSpecialCondition.LogoutBlock:

                    specialCondition = SpecialCondition.LogoutBlock;

                    break;

                case ConditionSpecialCondition.Drowning:

                    specialCondition = SpecialCondition.Drowning;                    

                    break;

                case ConditionSpecialCondition.Freezing:

                    specialCondition = SpecialCondition.Freezing;

                    break;

                case ConditionSpecialCondition.Dazzled:

                    specialCondition = SpecialCondition.Dazzled;

                    break;

                case ConditionSpecialCondition.Cursed:

                    specialCondition = SpecialCondition.Cursed;

                    break;

                case ConditionSpecialCondition.Bleeding:

                    specialCondition = SpecialCondition.Bleeding;

                    break;
            }

            if (specialCondition != SpecialCondition.None && !target.HasSpecialCondition(specialCondition) )
            {
                target.AddSpecialCondition(specialCondition);

                if (target is Player player)
                {
                    Context.Current.AddPacket(player.Client.Connection, new SetSpecialConditionOutgoingPacket(target.SpecialConditions) );
                }
            }

            condition.Update(target).Then( () =>
            {
                if (specialCondition != SpecialCondition.None && target.HasSpecialCondition(specialCondition) )
                {
                    target.RemoveSpecialCondition(specialCondition);

                    if (target is Player player)
                    {
                        Context.Current.AddPacket(player.Client.Connection, new SetSpecialConditionOutgoingPacket(target.SpecialConditions) );
                    }
                }

                server.Components.RemoveComponent(GameObject, this);

                return Promise.Completed;

            } ).Catch( (ex) =>
            {
                if (ex is PromiseCanceledException)
                {
                                
                }
                else
                {
                    server.Logger.WriteLine(ex.ToString(), LogLevel.Error);
                }

                if (specialCondition != SpecialCondition.None && target.HasSpecialCondition(specialCondition) )
                {
                    target.RemoveSpecialCondition(specialCondition);

                    if (target is Player player)
                    {
                        Context.Current.AddPacket(player.Client.Connection, new SetSpecialConditionOutgoingPacket(target.SpecialConditions) );
                    }
                }

                server.Components.RemoveComponent(GameObject, this);
            } );
        }

        public override void Stop(Server server)
        {
            condition.Stop(server);
        }
    }
}