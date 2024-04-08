using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseRemoveVipCommand : IncomingCommand
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
            Player.Vips.RemoveVip( (int)DatabasePlayerId);

            return Promise.Completed;
        }
    }
}