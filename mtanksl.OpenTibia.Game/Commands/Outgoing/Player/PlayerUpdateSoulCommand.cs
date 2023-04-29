using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateSoulCommand : Command
    {
        public PlayerUpdateSoulCommand(Player player, int soul)
        {
            Player = player;

            Soul = (byte)Math.Max(0, Math.Min(100, soul) );
        }

        public Player Player { get; set; }

        public byte Soul { get; set; }

        public override Promise Execute()
        {
            if (Player.Vocation == Vocation.Gamemaster)
            {
                return Promise.Completed;
            }

            if (Player.Soul != Soul)
            {
                Player.Soul = Soul;

                Context.AddPacket(Player.Client.Connection, new SendStatusOutgoingPacket(Player.Health, Player.MaxHealth, Player.Capacity, Player.Experience, Player.Level, Player.LevelPercent, Player.Mana, Player.MaxMana, Player.Skills.MagicLevel, Player.Skills.MagicLevelPercent, Player.Soul, Player.Stamina) );
              
                Context.AddEvent(Player, new PlayerUpdateSoulEventArgs(Player, Soul) );

                Context.AddEvent(new PlayerUpdateSoulEventArgs(Player, Soul) );
            }

            return Promise.Completed;
        }
    }
}