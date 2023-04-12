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
                Context.Server.MonsterFactory.Destroy(Monster);
            } );
        }
    }
}