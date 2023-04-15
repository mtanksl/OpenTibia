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
            return Context.AddCommand(new TileRemoveCreatureCommand(Monster.Tile, Monster) ).Then( () =>
            {
                if (Monster.Health == 0)
                {

                }

                Context.Server.MonsterFactory.Destroy(Monster);

                return Promise.Completed;
            } );
        }
    }
}