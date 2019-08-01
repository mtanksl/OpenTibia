using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class AttackBehaviour : Behaviour
    {
        public override void Start(Server server)
        {
            
        }

        public override void Update(Server server, Context context)
        {
            Creature creature = (Creature)GameObject;

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (creature != observer && creature.Tile.Position.IsInPlayerRange(observer.Tile.Position) )
                {
                    

                    break;
                }
            }
        }
    }
}