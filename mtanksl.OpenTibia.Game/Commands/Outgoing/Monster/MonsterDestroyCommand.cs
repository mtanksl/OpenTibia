using OpenTibia.Common.Objects;
using System;

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
            return context.AddCommand(new TileRemoveCreatureCommand(Monster.Tile, Monster) ).Then(ctx =>
            {
                ctx.Server.MonsterFactory.Destroy(Monster);
            } );            
        }
    }
}