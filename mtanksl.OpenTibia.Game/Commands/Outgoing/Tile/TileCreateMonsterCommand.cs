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
            Monster monster = Context.Server.MonsterFactory.Create(Name, Tile);

            if (monster != null)
            {
                Context.Server.MonsterFactory.Attach(monster);

                return Context.AddCommand(new TileAddCreatureCommand(Tile, monster) ).Then( () =>
                {
                    return Promise.FromResult(monster); 
                } );
            }

            return Promise.FromResult(monster); 
        }
    }
}