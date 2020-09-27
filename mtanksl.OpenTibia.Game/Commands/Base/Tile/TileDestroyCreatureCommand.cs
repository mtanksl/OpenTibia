using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class TileDestroyCreatureCommand : Command
    {
        public TileDestroyCreatureCommand(Tile tile, Creature creature)
        {
            Tile = tile;

            Creature = creature;
        }

        public Tile Tile { get; set; }

        public Creature Creature { get; set; }

        public override void Execute(Context context)
        {
            Command command = context.TransformCommand(new TileRemoveCreatureCommand(Tile, Creature) );

            command.Completed += (s, e) =>
            {
                if (Creature is Monster monster)
                {
                    context.Server.MonsterFactory.Destroy(monster);
                }
                else if (Creature is Npc npc)
                {
                    context.Server.NpcFactory.Destroy(npc);
                }

                base.OnCompleted(e.Context);
            };

            command.Execute(context);
        }
    }
}