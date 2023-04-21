using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class MonsterDestroyCommand : Command
    {
        public MonsterDestroyCommand(Monster monster)
        {
            Monster = monster;
        }

        public Monster Monster { get; set; }

        public override Promise Execute()
        {
            if (Context.Server.MonsterFactory.Detach(Monster) )
            {
                Context.Server.QueueForExecution( () =>
                {
                    Context.Server.MonsterFactory.Destroy(Monster);

                    return Context.AddCommand(new TileRemoveCreatureCommand(Monster.Tile, Monster) );
                } );
            }

            return Promise.Completed;
        }
    }
}