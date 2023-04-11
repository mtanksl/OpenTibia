using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class TileCreateMonsterCommand : CommandResult<Monster>
    {
        public TileCreateMonsterCommand(Tile tile, string name)
        {
            Tile = tile;

            Name = name;
        }

        public Tile Tile { get; set; }

        public string Name { get; set; }

        public override PromiseResult<Monster> Execute()
        {
            return Promise.Run<Monster>( (resolve, reject) =>
            {
                Monster monster = Context.Server.MonsterFactory.Create(Name);

                if (monster != null)
                {
                    Context.AddCommand(new TileAddCreatureCommand(Tile, monster) );
                }

                resolve(monster);
            } );
        }
    }
}