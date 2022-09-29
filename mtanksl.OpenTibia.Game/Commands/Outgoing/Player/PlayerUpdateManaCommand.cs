using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateManaCommand : Command
    {
        public PlayerUpdateManaCommand(Player player, ushort mana)
        {
            Player = player;

            Mana = mana;
        }

        public Player Player { get; set; }

        public ushort Mana { get; set; }

        public override void Execute(Context context)
        {
            if (Player.Mana != Mana)
            {
                Player.Mana = Mana;

                context.AddPacket(Player.Client.Connection, new SendStatusOutgoingPacket(Player.Health, Player.MaxHealth,

                                                                                           Player.Capacity,
                                                                                           
                                                                                           Player.Experience, Player.Level, Player.LevelPercent,
                                                                                           
                                                                                           Player.Mana, Player.MaxMana, 0, 0, Player.Soul,
                                                                                           
                                                                                           Player.Stamina) );
            }

            base.Execute(context);
        }
    }
}