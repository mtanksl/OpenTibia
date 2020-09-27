using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class TileCreateCreatureCommand : Command
    {
        public TileCreateCreatureCommand(Tile tile, string name)
        {
            Tile = tile;

            Name = name;
        }

        public Tile Tile { get; set; }

        public string Name { get; set; }

        public override void Execute(Context context)
        {
            Creature creature = context.Server.MonsterFactory.Create(Name);

            if (creature == null)
            {
                creature = context.Server.NpcFactory.Create(Name);
            }

            if (creature != null)
            {
                Command command = context.TransformCommand(new TileAddCreatureCommand(Tile, creature) );

                command.Completed += (s, e) =>
                {
                    base.OnCompleted(e.Context);
                };

                command.Execute(context);
            }
        }
    }
}