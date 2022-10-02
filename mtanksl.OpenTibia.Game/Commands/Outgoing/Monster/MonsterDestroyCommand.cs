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

        public override void Execute(Context context)
        {
            context.AddCommand(new TileRemoveCreatureCommand(Monster.Tile, Monster) ).Then(ctx =>
            {
                ctx.Server.MonsterFactory.Destroy(Monster);

                OnComplete(ctx);
            } );            
        }
    }
}