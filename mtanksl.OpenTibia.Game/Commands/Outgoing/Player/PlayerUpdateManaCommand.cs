using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateManaCommand : Command
    {
        public PlayerUpdateManaCommand(Player player, int mana) : this(player, mana, player.MaxMana)
        {

        }

        public PlayerUpdateManaCommand(Player player, int mana, int maxMana)
        {
            Player = player;

            Mana = (ushort)Math.Max(0, Math.Min(player.MaxMana, mana) );

            MaxMana = (ushort)Math.Max(0, Math.Min(player.MaxMana, maxMana) );
        }

        public Player Player { get; set; }

        public ushort Mana { get; set; }

        public ushort MaxMana { get; set; }

        public override Promise Execute()
        {
            if (Player.Mana != Mana || Player.MaxMana != MaxMana)
            {
                Player.Mana = Mana;

                Player.MaxMana = MaxMana;

                Context.AddPacket(Player.Client.Connection, new SendStatusOutgoingPacket(Player.Health, Player.MaxHealth, Player.Capacity, Player.Experience, Player.Level, Player.LevelPercent, Player.Mana, Player.MaxMana, Player.Skills.MagicLevel, Player.Skills.MagicLevelPercent, Player.Soul, Player.Stamina) );
            }

            return Promise.Completed;
        }
    }
}