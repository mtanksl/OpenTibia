using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class AttackBehaviour : PeriodicBehaviour
    {
        private Monster monster;

        public override void Start(Server server)
        {
            monster = (Monster)GameObject;            
        }

        private uint? targetId;

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
                    if (monster.Tile.Position.CanHearSay(observer.Tile.Position) )
                    {
                        targetId = observer.Id;

                        break;
                    }
                }
            }
            
            if (targetId != null)
            {
                var target = context.Server.GameObjects.GetCreature(targetId.Value);

                if (target == null)
                {
                    targetId = null;
                }
                else
                {
                    if ( !monster.Tile.Position.CanHearSay(target.Tile.Position) )
                    {
                        targetId = null;
                    }
                    else
                    {
                        context.AddCommand(new CombatTargetedAttackCommand(monster, target, ProjectileType.Spear, null, _ => -Server.Random.Next(0, 10) ) );
                    }
                }
            }
        }

        public override void Stop(Server server)
        {
            
        }        
    }
}