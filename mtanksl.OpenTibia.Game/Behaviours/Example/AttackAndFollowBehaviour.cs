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

        private static Random random = new Random();

        private DateTime lastAttack;

        public override void Update(Context context)
        {
            if ( (DateTime.UtcNow - lastAttack).TotalMilliseconds < 1000)
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
                        if (target is Npc || target is Player)
                        {
                            context.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouMayNotAttackThisCreature),

                                                                          new StopAttackAndFollowOutgoingPacket(0) );

                            Stop();
                        }
                        else
                        {
                            if (state == State.Attack || state == State.AttackAndFollow)
                            {
                                context.AddCommand(new ShowProjectileCommand(player.Tile.Position, target.Tile.Position, ProjectileType.Spear) );

                                int damage = random.Next(0, 20);

                                if (damage > 0)
                                {
                                    context.AddCommand(new ShowAnimatedTextCommand(target.Tile.Position, AnimatedTextColor.Red, damage.ToString() ) );

                                    if (target.Health > damage)
                                    {
                                        context.AddCommand(new CreatureUpdateHealthCommand(target, (ushort)(target.Health - damage), target.MaxHealth) );
                                    }
                                    else
                                    {
                                        context.AddCommand(new TileCreateItemCommand(target.Tile, 3058, 1) ).Then( (ctx, item) =>
                                        {
                                            ctx.AddCommand(new ItemDecayTransformCommand(item, 10000, 3059, 1) ).Then( (ctx2, item2) =>
                                            {
                                                ctx2.AddCommand(new ItemDecayTransformCommand(item2, 10000, 3060, 1) ).Then( (ctx3, item3) =>
                                                {
                                                    ctx3.AddCommand(new ItemDecayDestroyCommand(item3, 10000) );
                                                } );
                                            } );
                                        } );

                                        context.AddCommand(new MonsterDestroyCommand( (Monster)target) );

                                        context.AddPacket(player.Client.Connection, new StopAttackAndFollowOutgoingPacket(0) );

                                        Stop();
                                    }
                                }
                                else
                                {
                                    context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.Puff) );
                                }
                            }
                            
                            if (state == State.Follow || state == State.AttackAndFollow)
                            {
                                //TODO
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