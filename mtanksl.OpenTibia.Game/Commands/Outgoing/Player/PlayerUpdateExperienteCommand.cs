using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateExperienteCommand : Command
    {
        public PlayerUpdateExperienteCommand(Player player, uint experience, ushort level, byte levelPercent)
        {
            Player = player;

            Experience = experience;

            Level = level;

            LevelPercent = levelPercent;
        }

        public Player Player { get; set; }

        public uint Experience { get; set; }

        public ushort Level { get; set; }

        public byte LevelPercent { get; set; }

        public override void Execute(Context context)
        {
            if (Player.Experience != Experience || Player.Level != Level || Player.LevelPercent != LevelPercent)
            {
                Player.Experience = Experience;

                Player.Level = Level;

                Player.LevelPercent = LevelPercent;

                context.AddPacket(Player.Client.Connection, new SendStatusOutgoingPacket(Player.Health, Player.MaxHealth, Player.Capacity, Player.Experience, Player.Level, Player.LevelPercent, Player.Mana, Player.MaxMana, 0, 0, Player.Soul, Player.Stamina) );
            }

            OnComplete(context);
        }
    }
}