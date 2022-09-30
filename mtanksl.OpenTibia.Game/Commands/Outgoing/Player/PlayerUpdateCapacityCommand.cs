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

        public override void Execute(Context context)
        {
            if (Player.Capacity != Capacity)
            {
                Player.Capacity = Capacity;

                context.AddPacket(Player.Client.Connection, new SendStatusOutgoingPacket(Player.Health, Player.MaxHealth,

                                                                                           Player.Capacity,
                                                                                           
                                                                                           Player.Experience, Player.Level, Player.LevelPercent,
                                                                                           
                                                                                           Player.Mana, Player.MaxMana, 0, 0, Player.Soul,
                                                                                           
                                                                                           Player.Stamina) );
            }

            OnComplete(context);
        }
    }
}