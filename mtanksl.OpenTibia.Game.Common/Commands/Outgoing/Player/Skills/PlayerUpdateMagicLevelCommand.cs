using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateMagicLevelCommand : Command
    {
        public PlayerUpdateMagicLevelCommand(Player player, byte magicLevel, byte magicLevelPercent)
        {
            Player = player;

            MagicLevel = magicLevel;

            MagicLevelPercent = magicLevelPercent;
        }

        public Player Player { get; set; }

        public byte MagicLevel { get; set; }

        public byte MagicLevelPercent { get; set; }

        public override Promise Execute()
        {
            if (Player.Skills.MagicLevel != MagicLevel || Player.Skills.MagicLevelPercent != MagicLevelPercent)
            {
                Player.Skills.MagicLevel = MagicLevel;

                Player.Skills.MagicLevelPercent = MagicLevelPercent;

                Context.AddPacket(Player, new SendStatusOutgoingPacket(Player.Health, Player.MaxHealth, Player.Capacity, Player.Experience, Player.Level, Player.LevelPercent, Player.Mana, Player.MaxMana, Player.Skills.MagicLevel, Player.Skills.MagicLevelPercent, Player.Soul, Player.Stamina) );
            }

            return Promise.Completed;
        }
    }
}