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

        public override void Execute(Server server, CommandContext context)
        {
            server.Map.RemoveCreature(Creature);

            new TileRemoveCreatureCommand(Creature.Tile, Creature.Tile.GetIndex(Creature) ).Execute(server, context);

            base.Execute(server, context);            
        }
    }
}