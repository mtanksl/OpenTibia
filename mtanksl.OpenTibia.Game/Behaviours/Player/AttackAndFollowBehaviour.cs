using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Strategies;
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

        public override void Start(Server server)
        {
            player = (Player)GameObject;            
        }

        private State state;

        private uint? targetId;

        public void Attack(Creature creature)
        {
            state = State.Attack;

            targetId = creature.Id;
        }

        public void Follow(Creature creature)
        {
            state = State.Follow;

            targetId = creature.Id;
        }

        public void AttackAndFollow(Creature creature)
        {
            state = State.AttackAndFollow;

            targetId = creature.Id;
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
            state = State.None;

            targetId = null;
        }

        private IAttackStrategy attackStrategy = new CloseAttackStrategy(1000, (attacker, target) => -Server.Random.Next(0, 50) );

        private IWalkStrategy walkStrategy = new FollowWalkStrategy();

        private DateTime attackCooldown;

        private DateTime moveCooldown;

        public override void Update(Context context)
        {
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
                            if (state == State.Follow || state == State.AttackAndFollow)
                            {
                                if (DateTime.UtcNow > moveCooldown)
                                {
                                    var toTile = walkStrategy.GetNext(context, null, player, target);

                                    if (toTile != null)
                                    {
                                        context.AddCommand(new CreatureUpdateParentCommand(player, toTile) );

                                        moveCooldown = DateTime.UtcNow.AddMilliseconds(1000 * toTile.Ground.Metadata.Speed / player.Speed);
                                    }
                                    else
                                    {
                                        moveCooldown = DateTime.UtcNow.AddMilliseconds(1000 * player.Tile.Ground.Metadata.Speed / player.Speed);
                                    }
                                }
                            }

                            if (state == State.Attack || state == State.AttackAndFollow)
                            {
                                if (DateTime.UtcNow > attackCooldown)
                                {
                                    var command = attackStrategy.GetNext(context, player, target);

                                    if (command != null)
                                    {
                                        context.AddCommand(command);
                                    } 
                                    
                                    attackCooldown = DateTime.UtcNow.AddMilliseconds(attackStrategy.CooldownInMilliseconds);
                                }
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