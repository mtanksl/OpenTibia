using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Strategies;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

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

        private IAttackStrategy attackStrategy;

        private IWalkStrategy walkStrategy;

        public AttackAndFollowBehaviour(IAttackStrategy attackStrategy, IWalkStrategy walkStrategy)
        {
            this.attackStrategy = attackStrategy;

            this.walkStrategy = walkStrategy;
        }

        private Player player;

        public override void Start(Server server)
        {
            player = (Player)GameObject;
        }

        private State state;

        private Creature target;

        public void Attack(Creature creature)
        {
            state = State.Attack;

            target = creature;
        }

        public void Follow(Creature creature)
        {
            state = State.Follow;

            target = creature;
        }

        public void AttackAndFollow(Creature creature)
        {
            state = State.AttackAndFollow;

            target = creature;
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

            target = null;
        }

        private DateTime attackCooldown;

        private DateTime moveCooldown;

        public override Promise Update()
        {
            if (target != null)
            {
                if (target.Tile == null)
                {
                    Context.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TargetLost),

                                                                new StopAttackAndFollowOutgoingPacket(0) );

                    Stop();
                }
                else
                {
                    if ( !player.Tile.Position.CanHearSay(target.Tile.Position) )
                    {
                        Context.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TargetLost),

                                                                    new StopAttackAndFollowOutgoingPacket(0) );

                        Stop();
                    }
                    else
                    {
                        List<Command> commands = new List<Command>();

                        if (state == State.Follow || state == State.AttackAndFollow)
                        {
                            if (DateTime.UtcNow > moveCooldown)
                            {
                                var toTile = walkStrategy.GetNext(null, player, target);

                                if (toTile != null)
                                {
                                    commands.Add(new CreatureUpdateParentCommand(player, toTile) );

                                    moveCooldown = DateTime.UtcNow.AddMilliseconds(1000 * toTile.Ground.Metadata.Speed / player.Speed);
                                }
                            }
                        }

                        if (state == State.Attack || state == State.AttackAndFollow)
                        {
                            if (DateTime.UtcNow > attackCooldown)
                            {
                                var command = attackStrategy.GetNext(player, target);

                                if (command != null)
                                {
                                    commands.Add(command);

                                    attackCooldown = DateTime.UtcNow.AddMilliseconds(attackStrategy.CooldownInMilliseconds);
                                } 
                            }
                        }

                        return Command.WhenAll(commands.ToArray() );
                    }
                }
            }

            return Promise.Completed;
        }

        public override void Stop(Server server)
        {
            
        }        
    }
}