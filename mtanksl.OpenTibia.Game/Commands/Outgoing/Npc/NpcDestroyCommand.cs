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
            if (Npc.Tile != null)
            {
                context.AddCommand(new TileRemoveCreatureCommand(Npc.Tile, Npc) ).Then(ctx =>
                {
                    ctx.Server.NpcFactory.Destroy(Npc);

                    OnComplete(ctx);
                } );
            }
            else
            {
                context.Server.NpcFactory.Destroy(Npc);

                OnComplete(context);
            }            
        }
    }
}