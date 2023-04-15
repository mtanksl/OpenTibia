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
            return Context.AddCommand(new TileRemoveCreatureCommand(Npc.Tile, Npc) ).Then( () =>
            {
                Context.Server.NpcFactory.Destroy(Npc);

                return Promise.Completed;
            } );
        }
    }
}