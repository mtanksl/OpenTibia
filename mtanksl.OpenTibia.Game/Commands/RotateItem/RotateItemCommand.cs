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

        protected bool IsNextTo(Tile fromTile, Context context)
        {
            if ( !Player.Tile.Position.IsNextTo(fromTile.Position) )
            {
                WalkToUnknownPathCommand walkToUnknownPathCommand = new WalkToUnknownPathCommand(Player, fromTile);

                walkToUnknownPathCommand.Completed += (s, e) =>
                {
                    context.Server.QueueForExecution(Constants.CreatureActionSchedulerEvent(Player), Constants.CreatureActionSchedulerEventDelay, this);
                };

                walkToUnknownPathCommand.Execute(context);

                return false;
            }

            return true;
        }

        protected bool IsRotatable(Item fromItem, Context context)
        {
            if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Rotatable) )
            {
                return false;
            }

            return true;
        }

        protected void RotateItem(Item fromItem, Context context)
        {
            IItemRotateScript script;

            if ( !context.Server.Scripts.ItemRotateScripts.TryGetValue(fromItem.Metadata.OpenTibiaId, out script) || !script.OnItemRotate(Player, fromItem, context) )
            {
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisItem) );
            }
            else
            {
                base.Execute(context);
            }
        }
    }
}