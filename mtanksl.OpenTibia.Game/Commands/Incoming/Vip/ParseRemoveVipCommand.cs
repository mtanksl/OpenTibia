using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseRemoveVipCommand : Command
    {
        public ParseRemoveVipCommand(Player player, uint creatureId)
        {
            Player = player;

            CreatureId = creatureId;
        }

        public Player Player { get; set; }

        public uint CreatureId { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                Player.Client.VipCollection.RemoveVip(CreatureId);

                resolve();
            } );
        }
    }
}