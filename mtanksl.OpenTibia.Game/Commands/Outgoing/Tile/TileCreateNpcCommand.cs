using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class TileCreateNpcCommand : CommandResult<Npc>
    {
        public TileCreateNpcCommand(Tile tile, string name)
        {
            Tile = tile;

            Name = name;
        }

        public Tile Tile { get; set; }

        public string Name { get; set; }

        public override PromiseResult<Npc> Execute(Context context)
        {
            return PromiseResult<Npc>.Run(resolve =>
            {
                Npc npc = context.Server.NpcFactory.Create(Name);

                if (npc != null)
                {
                    context.AddCommand(new TileAddCreatureCommand(Tile, npc) ).Then(ctx =>
                    {
                        resolve(ctx, npc);
                    } );
                }
            } );
        }
    }
}