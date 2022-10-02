using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Extensions
{
    public static class MonsterExtensions
    {
        public static Promise Destroy(this Monster monster)
        {
            Context context = Context.Current;

            return context.AddCommand(new MonsterDestroyCommand(monster) );
        }
    }
}