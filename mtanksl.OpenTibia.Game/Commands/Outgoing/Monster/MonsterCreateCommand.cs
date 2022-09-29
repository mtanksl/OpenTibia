using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class MonsterCreateCommand : Command
    {
        public MonsterCreateCommand(Tile tile, string name)
        {
            Tile = tile;

            Name = name;
        }

        public Tile Tile { get; set; }

        public string Name { get; set; }

        public override void Execute(Context context)
        {
            Monster monster = context.Server.MonsterFactory.Create(Name);

            if (monster != null)
            {
                context.AddCommand(new TileAddCreatureCommand(Tile, monster), ctx =>
                {
                    base.Execute(ctx);
                } );
            }
        }
    }
}