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
            return Promise.Run( (resolve, reject) =>
            {
                context.AddCommand(new TileRemoveCreatureCommand(Monster.Tile, Monster) );

                context.Server.MonsterFactory.Destroy(Monster);

                resolve(context);
            } );   
        }
    }
}