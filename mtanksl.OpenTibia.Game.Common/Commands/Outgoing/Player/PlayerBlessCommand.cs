using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerBlessCommand : Command
    {
        public PlayerBlessCommand(Player player, string message, string blessName)
        {
            Player = player;

            Message = message;

            BlessName = blessName;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public string BlessName { get; set; }

        public override Promise Execute()
        {
            if (Player.Blesses.HasBless(BlessName) )
            {
                return Context.AddCommand(new ShowTextCommand(Player, TalkType.MonsterSay, "You already possess this blessing.") ).Then( () =>
                {
                    return Promise.Break;
                } );
            }
            else
            {
                Player.Blesses.SetBless(BlessName);

                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, Message) );

                return Promise.Completed;
            }
        }
    }
}
