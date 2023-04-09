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

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                context.AddCommand(new TileRemoveCreatureCommand(Npc.Tile, Npc) );

                context.Server.NpcFactory.Destroy(Npc);

                resolve(context);
            } ); 
        }
    }
}