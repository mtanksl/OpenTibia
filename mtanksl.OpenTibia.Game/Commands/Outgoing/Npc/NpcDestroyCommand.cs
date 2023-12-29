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
            if (Context.Server.NpcFactory.Detach(Npc) )
            {
                Context.Server.QueueForExecution( () =>
                {
                    Context.Server.NpcFactory.ClearComponentsAndEventHandlers(Npc);

                    return Context.AddCommand(new TileRemoveCreatureCommand(Npc.Tile, Npc) );
                } );
            }

            return Promise.Completed;
        }
    }
}