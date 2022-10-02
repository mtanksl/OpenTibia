using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Extensions
{
    public static class NpcExtensions
    {
        public static Promise Destroy(this Npc npc)
        {
            Context context = Context.Current;

            return context.AddCommand(new NpcDestroyCommand(npc) );
        }
    }
}