using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Extensions
{
    public static class MonsterExtensions
    {
        /// <exception cref="InvalidOperationException"></exception>

        public static Promise Say(this Monster monster, string message)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new MonsterSayCommand(monster, message) );
        }
    }
}