using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateMagicLevelCommand : Command
    {
        public PlayerUpdateMagicLevelCommand(Player player, ulong magicLevelPoints, byte magicLevel, byte magicLevelPercent)
        {
            Player = player;

            MagicLevelPoints = magicLevelPoints;

            MagicLevel = magicLevel;

            MagicLevelPercent = magicLevelPercent;
        }

        public Player Player { get; set; }

        public ulong MagicLevelPoints { get; set; }

        public byte MagicLevel { get; set; }

        public byte MagicLevelPercent { get; set; }

        public override Promise Execute()
        {
            if (Player.Skills.MagicLevelPoints != MagicLevelPoints || Player.Skills.MagicLevel != MagicLevel || Player.Skills.MagicLevelPercent != MagicLevelPercent)
            {
                Player.Skills.MagicLevelPoints = MagicLevelPoints;

                if (Player.Skills.MagicLevel != MagicLevel || Player.Skills.MagicLevelPercent != MagicLevelPercent)
                {
                    Player.Skills.MagicLevel = MagicLevel;

                    Player.Skills.MagicLevelPercent = MagicLevelPercent;

                    Context.AddPacket(Player, new SendStatusOutgoingPacket(Player.Health, Player.MaxHealth, Player.Capacity, Player.Experience, Player.Level, Player.LevelPercent, Player.Mana, Player.MaxMana, Player.Skills.MagicLevel, Player.Skills.MagicLevelPercent, Player.Soul, Player.Stamina) );
                }

                Context.AddEvent(new PlayerUpdateMagicLevelEventArgs(Player, MagicLevelPoints, MagicLevel) );
            }

            return Promise.Completed;
        }
    }
}