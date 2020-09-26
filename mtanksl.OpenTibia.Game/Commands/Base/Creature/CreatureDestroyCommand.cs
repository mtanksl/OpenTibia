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
            //Arrange

            //Act

            new TileRemoveCreatureCommand(Creature.Tile, Creature).Execute(context);

            context.Server.Map.RemoveCreature(Creature);

            foreach (var component in Creature.GetComponents<Behaviour>() )
            {
                component.Stop(context.Server);
            }

            //Notify

            base.Execute(context);            
        }
    }
}