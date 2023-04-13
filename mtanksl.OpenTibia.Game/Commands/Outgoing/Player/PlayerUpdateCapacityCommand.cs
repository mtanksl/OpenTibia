using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateCapacityCommand : Command
    {
        public PlayerUpdateCapacityCommand(Player player, int capacity)
        {
            Player = player;

            Capacity = (uint)Math.Max(0, capacity);
        }

        public Player Player { get; set; }

        public uint Capacity { get; set; }

        public override Promise Execute()
        {
            if (Player.Capacity != Capacity)
            {
                Player.Capacity = Capacity;

                Context.AddPacket(Player.Client.Connection, new SendStatusOutgoingPacket(Player.Health, Player.MaxHealth, Player.Capacity, Player.Experience, Player.Level, Player.LevelPercent, Player.Mana, Player.MaxMana, Player.Skills.MagicLevel, Player.Skills.MagicLevelPercent, Player.Soul, Player.Stamina) );
            }

            return Promise.Completed;
        }
    }
}