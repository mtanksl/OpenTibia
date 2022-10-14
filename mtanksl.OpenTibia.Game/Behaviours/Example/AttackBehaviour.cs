using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Components
{
    public class AttackBehaviour : PeriodicBehaviour
    {
        private Monster attacker;

        private uint? targetId;

        public override void Start(Server server)
        {
            attacker = (Monster)GameObject;            
        }

        private static Random random = new Random();

        private DateTime lastAttack;

        public override void Update(Context context)
        {
            if ( (DateTime.UtcNow - lastAttack).TotalMilliseconds < 2000)
            {
                return;
            }

            lastAttack = DateTime.UtcNow;

            if (targetId == null)
            {
                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    if (attacker.Tile.Position.CanHearSay(observer.Tile.Position) )
                    {
                        targetId = observer.Id;

                        break;
                    }
                }
            }
            
            if (targetId != null)
            {
                var target = context.Server.GameObjects.GetGameObject<Creature>(targetId.Value);

                if (target == null)
                {
                    targetId = null;
                }
                else
                {
                    if ( !attacker.Tile.Position.CanHearSay(target.Tile.Position) )
                    {
                        targetId = null;
                    }
                    else
                    {
                        context.AddCommand(new ShowProjectileCommand(attacker.Tile.Position, target.Tile.Position, ProjectileType.Spear) );

                        context.AddPacket( ( (Player)target).Client.Connection, new SetFrameColorOutgoingPacket(attacker.Id, FrameColor.Black) );

                        int damage = random.Next(0, 10);

                        if (damage > 0)
                        {
                            context.AddCommand(new ShowAnimatedTextCommand(target.Tile.Position, AnimatedTextColor.Red, damage.ToString() ) );

                            if (target.Health > damage)
                            {
                                context.AddCommand(new CreatureUpdateHealthCommand(target, (ushort)(target.Health - damage), target.MaxHealth) );

                                context.AddPacket( ( (Player)target).Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + damage + " hitpoints due to an attack by " + attacker.Name) );
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

                                context.AddCommand(new PlayerDestroyCommand( (Player)target) ).Then(ctx =>
                                {
                                    ctx.Disconnect( ( (Player)target).Client.Connection);
                                } );

                                targetId = null;
                            }
                        }
                        else
                        {
                            context.AddCommand(new ShowMagicEffectCommand(attacker.Tile.Position, MagicEffectType.Puff) );
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