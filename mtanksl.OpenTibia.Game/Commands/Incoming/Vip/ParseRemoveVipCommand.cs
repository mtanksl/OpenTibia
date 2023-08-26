using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseRemoveVipCommand : Command
    {
        public ParseRemoveVipCommand(Player player, uint databasePlayerId)
        {
            Player = player;

            DatabasePlayerId = databasePlayerId;
        }

        public Player Player { get; set; }

        public uint DatabasePlayerId { get; set; }

        public override Promise Execute()
        {
            Player.Client.Vips.RemoveVip( (int)DatabasePlayerId);

            return Promise.Completed;
        }
    }
}