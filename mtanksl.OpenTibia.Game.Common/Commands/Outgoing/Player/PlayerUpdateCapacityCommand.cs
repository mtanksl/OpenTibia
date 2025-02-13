using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateCapacityCommand : Command
    {
        public PlayerUpdateCapacityCommand(Player player, uint capacity)
        {
            Player = player;

            Capacity = (ushort)Math.Max(0, Math.Min(player.MaxCapacity, capacity) );
        }

        public Player Player { get; set; }

        public uint Capacity { get; set; }

        public override Promise Execute()
        {
            if (Player.Capacity != Capacity)
            {
                Player.Capacity = Capacity;

                Context.AddPacket(Player, new SendStatusOutgoingPacket(
                        Player.Health, Player.MaxHealth, 
                        Player.Capacity, 
                        Player.Experience, Player.Level, Player.LevelPercent, 
                        Player.Mana, Player.MaxMana, 
                        Player.Skills.GetSkillLevel(Skill.MagicLevel), Player.Skills.GetSkillPercent(Skill.MagicLevel), 
                        Player.Soul, 
                        Player.Stamina) );

                Context.AddEvent(new PlayerUpdateCapacityEventArgs(Player, Capacity) );
            }

            return Promise.Completed;
        }
    }
}