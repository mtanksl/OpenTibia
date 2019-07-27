using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Scripts;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public abstract class RotateItemCommand : Command
    {
        public RotateItemCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        protected void RotateItem(Item fromItem, Server server, CommandContext context)
        {
            ItemRotateScript script;

            if ( !server.ItemRotateScripts.TryGetValue(fromItem.Metadata.OpenTibiaId, out script) || !script.Execute(Player, fromItem, server, context) )
            {
                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisItem) );
            }
            else
            {
                base.Execute(server, context);
            }
        }
    }
}