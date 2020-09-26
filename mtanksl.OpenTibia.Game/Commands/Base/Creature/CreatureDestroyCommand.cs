using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.Commands
{
    public class CreatureDestroyCommand : Command
    {
        public CreatureDestroyCommand(Creature creature)
        {
            Creature = creature;
        }

        public Creature Creature { get; set; }

        public override void Execute(Context context)
        {
            new TileRemoveCreatureCommand(Creature.Tile, Creature).Execute(context);

            context.Server.GameObjects.RemoveGameObject(Creature);

            foreach (var component in Creature.GetComponents<Behaviour>() )
            {
                component.Stop(context.Server);
            }

            base.OnCompleted(context);            
        }
    }
}