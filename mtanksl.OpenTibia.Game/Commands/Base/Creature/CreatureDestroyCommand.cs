using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class CreatureDestroyCommand : Command
    {
        public CreatureDestroyCommand(Creature creature)
        {
            Creature = creature;
        }

        public Creature Creature { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            //Act

            new TileRemoveCreatureCommand(Creature.Tile, Creature).Execute(server, context);
            
            server.Map.RemoveCreature(Creature);

            //Notify

            base.Execute(server, context);            
        }
    }
}