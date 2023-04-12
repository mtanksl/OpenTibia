using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateCapacityCommand : Command
    {
        public PlayerUpdateCapacityCommand(Player player, uint capacity)
        {
            Player = player;

            Capacity = capacity;
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