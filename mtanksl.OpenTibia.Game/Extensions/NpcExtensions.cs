using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Extensions
{
    public static class NpcExtensions
    {
        /// <exception cref="InvalidOperationException"></exception>

        public static Promise Say(this Npc npc, string message)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new NpcSayCommand(npc, message) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise SayToPlayer(this Npc npc, Player player, string message)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new NpcSayToPlayerCommand(npc, player, message) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise Trade(this Npc npc, Player player, List<OfferDto> offers)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new NpcTradeCommand(npc, player, offers) );
        }
    }
}