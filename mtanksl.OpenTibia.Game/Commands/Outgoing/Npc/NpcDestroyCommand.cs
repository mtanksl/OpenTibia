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

        public override Promise Execute(Context context)
        {
            return context.AddCommand(new TileRemoveCreatureCommand(Npc.Tile, Npc) ).Then( (ctx, index) =>
            {
                ctx.Server.NpcFactory.Destroy(Npc);
            } );
        }
    }
}