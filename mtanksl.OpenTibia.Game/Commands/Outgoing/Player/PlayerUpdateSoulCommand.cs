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

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                if (Player.Soul != Soul)
                {
                    Player.Soul = Soul;

                    Context.AddPacket(Player.Client.Connection, new SendStatusOutgoingPacket(Player.Health, Player.MaxHealth, Player.Capacity, Player.Experience, Player.Level, Player.LevelPercent, Player.Mana, Player.MaxMana, Player.Skills.MagicLevel, Player.Skills.MagicLevelPercent, Player.Soul, Player.Stamina) );
                }

                resolve();
            } );
        }
    }
}