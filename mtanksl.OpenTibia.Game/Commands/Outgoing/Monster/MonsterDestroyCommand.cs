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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                context.AddCommand(new TileRemoveCreatureCommand(Monster.Tile, Monster) );

                context.Server.MonsterFactory.Destroy(Monster);

                resolve(context);
            } );   
        }
    }
}