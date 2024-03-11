using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Extensions
{
    public static class NpcExtensions
    {
        /// <exception cref="InvalidOperationException"></exception>

        public static Promise Destroy(this Npc npc)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new NpcDestroyCommand(npc) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise Say(this Npc player, string message, bool npcChannel)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new NpcSayCommand(player, message, npcChannel) );
        }
    }
}