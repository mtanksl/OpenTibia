using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

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

        public override PromiseResult<Npc> Execute()
        {
            Npc npc = Context.Server.NpcFactory.Create(Name, Tile);

            if (npc != null)
            {
                Context.Server.NpcFactory.Attach(npc);

                return Context.AddCommand(new TileAddCreatureCommand(Tile, npc) ).Then( () =>
                {
                    return Promise.FromResult(npc);
                } );
            }

            return Promise.FromResult(npc);
        }
    }
}