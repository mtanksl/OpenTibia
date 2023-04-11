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
                Context.AddCommand(new TileRemoveCreatureCommand(Npc.Tile, Npc) );

                Context.Server.NpcFactory.Destroy(Npc);

                resolve();
            } ); 
        }
    }
}