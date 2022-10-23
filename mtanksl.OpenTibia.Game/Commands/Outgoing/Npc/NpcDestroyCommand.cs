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
            return Promise.Run(resolve =>
            {
                context.AddCommand(new TileRemoveCreatureCommand(Npc.Tile, Npc) );

                context.Server.NpcFactory.Destroy(Npc);

                resolve(context);
            } ); 
        }
    }
}