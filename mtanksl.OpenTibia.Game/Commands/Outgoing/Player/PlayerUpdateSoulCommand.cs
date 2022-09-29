using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateSoulCommand : Command
    {
        public PlayerUpdateSoulCommand(Player player, byte soul)
        {
            Player = player;

            Soul = soul;
        }

        public Player Player { get; set; }

        public byte Soul { get; set; }

        public override void Execute(Context context)
        {
            if (Player.Soul != Soul)
            {
                Player.Soul = Soul;

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