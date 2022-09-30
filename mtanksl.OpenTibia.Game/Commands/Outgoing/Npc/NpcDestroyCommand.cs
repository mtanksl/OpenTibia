using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class NpcDestroyCommand : Command
    {
        public NpcDestroyCommand(Npc npc)
        {
            Npc = npc;
        }

        public Npc Npc { get; set; }

        public override void Execute(Context context)
        {
            context.AddCommand(new TileRemoveCreatureCommand(Npc.Tile, Npc), ctx =>
            {
                context.Server.NpcFactory.Destroy(Npc);

                OnComplete(ctx);
            } );
        }
    }
}