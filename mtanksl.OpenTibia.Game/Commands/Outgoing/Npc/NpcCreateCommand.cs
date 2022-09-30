using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class NpcCreateCommand : CommandResult<Npc>
    {
        public NpcCreateCommand(Tile tile, string name)
        {
            Tile = tile;

            Name = name;
        }

        public Tile Tile { get; set; }

        public string Name { get; set; }

        public override void Execute(Context context)
        {
            Npc npc = context.Server.NpcFactory.Create(Name);

            if (npc != null)
            {
                context.AddCommand(new TileAddCreatureCommand(Tile, npc) ).Then(ctx =>
                {
                    OnComplete(ctx, npc);
                } );
            }
        }
    }
}