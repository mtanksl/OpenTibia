using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;

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

        private Player player;

        private uint? targetId;

        private State state;

        private DateTime lastAttack;

        public override void Start(Server server)
        {
            player = (Player)GameObject;            
        }

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

        public override void Update(Context context)
        {
            if ( (DateTime.UtcNow - lastAttack).TotalMilliseconds < 500)
            {
                return;
            }

            lastAttack = DateTime.UtcNow;

            if (targetId != null)
            {
                var target = context.Server.GameObjects.GetCreature(targetId.Value);

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
                        if (target is Npc)
                        {
                            context.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouMayNotAttackThisCreature),

                                                                        new StopAttackAndFollowOutgoingPacket(0) );

                            Stop();
                        }
                        else
                        {
                            if (state == State.Attack || state == State.AttackAndFollow)
                            {
                                int health = Server.Random.Next(0, 20);

                                if (health > 0)
                                {
                                    context.AddCommand(new CombatDistanceAttackCommand(player, target, ProjectileType.Spear, -health) );
                                }
                                else
                                {
                                    context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.Puff) );
                                }
                            }
                            
                            if (state == State.Follow || state == State.AttackAndFollow)
                            {
                                //TODO: Follow
                            }
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