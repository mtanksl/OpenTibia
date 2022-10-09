using OpenTibia.Common.Objects;
using System;

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

        public override PromiseResult<Monster> Execute(Context context)
        {
            return PromiseResult<Monster>.Run(resolve =>
            {
                Monster monster = context.Server.MonsterFactory.Create(Name);

                if (monster != null)
                {
                    context.AddCommand(new TileAddCreatureCommand(Tile, monster) ).Then( (ctx, index) =>
                    {
                        resolve(ctx, monster);
                    } );
                }
            } );
        }
    }
}