using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUseItemWithCreatureCommand : Command
    {
        public PlayerUseItemWithCreatureCommand(IncomingCommand source, Player player, Item item, Creature toCreature)
        {
            Source = source;

            Player = player;

            Item = item;

            ToCreature = toCreature;
        }

        public IncomingCommand Source { get; set; }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public Creature ToCreature { get; set; }

        public override Promise Execute()
        {
            Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );

            return Promise.Break;
        }
    }
}