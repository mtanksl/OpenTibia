using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class RemoveVipCommand : Command
    {
        public RemoveVipCommand(Player player, uint creatureId)
        {
            Player = player;

            CreatureId = creatureId;
        }

        public Player Player { get; set; }

        public uint CreatureId { get; set; }

        public override void Execute(Context context)
        {
            base.OnCompleted(context);
        }
    }
}