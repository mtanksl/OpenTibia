using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Components
{
    public class AttackAndFollowBehaviour : PeriodicBehaviour
    {
        private enum State
        {
            None,

            Attack,

            Follow,

            AttackAndFollow
        }

        private uint? targetId;

        private State state;

        public void Attack(Creature creature)
        {
            targetId = creature.Id;

            state = State.Attack;
        }

        public void Follow(Creature creature)
        {
            targetId = creature.Id;

            state = State.Follow;
        }

        public void AttackAndFollow(Creature creature)
        {
            targetId = creature.Id;

            state = State.AttackAndFollow;
        }

        public void StartFollow()
        {
            if (state == State.Attack)
            {
                state = State.AttackAndFollow;
            }
        }

        public void StopFollow()
        {
            if (state == State.AttackAndFollow)
            {
                state = State.Attack;
            }
        }

        public void Stop()
        {
            targetId = null;

            state = State.None;
        }

        private Player player;

        public override void Start(Server server)
        {
            player = (Player)GameObject;            
        }

        public override void Update(Context context)
        {
            if (targetId != null)
            {
                var target = context.Server.GameObjects.GetGameObject<Creature>(targetId.Value);

                if (target == null)
                {
                    context.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TargetLost),

                                                                new StopAttackAndFollowOutgoingPacket(0) );

                    Stop();
                }
                else
                {
                    if ( !player.Tile.Position.CanHearSay(target.Tile.Position) )
                    {
                        context.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TargetLost),

                                                                    new StopAttackAndFollowOutgoingPacket(0) );

                        Stop();
                    }
                    else
                    {
                        //TODO: Attack and Follow

                        switch (state)
                        {
                            case State.Attack:
                            { 
                                context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.RedShimmer) );
                            }

                            break;

                            case State.Follow:
                            {
                                context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.GreenShimmer) );
                            }

                            break;

                            case State.AttackAndFollow:
                            { 
                                context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.BlueShimmer) );
                            }

                            break;
                        }
                    }
                }
            }
        }

        public override void Stop(Server server)
        {
            
        }        
    }
}