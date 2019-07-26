using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Scripts;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithItemFromTileToTileCommand : Command
    {
        public UseItemWithItemFromTileToTileCommand(Player player, Position fromPosition, byte fromIndex, ushort fromItemId, Position toPosition, byte toIndex, ushort toItemId)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            FromItemId = fromItemId;

            ToPosition = toPosition;

            ToIndex = ToIndex;

            ToItemId = toItemId;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort FromItemId { get; set; }

        public Position ToPosition { get; set; }

        public byte ToIndex { get; set; }

        public ushort ToItemId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == FromItemId)
                {
                    Tile toTile = server.Map.GetTile(ToPosition);

                    if (toTile != null)
                    {
                        Item toItem = toTile.GetContent(ToIndex) as Item;

                        if (toItem != null && toItem.Metadata.TibiaId == ToItemId)
                        {
                            if ( !Player.Tile.Position.IsNextTo(fromTile.Position) )
                            {
                                MoveDirection[] moveDirections = server.Pathfinding.GetMoveDirections(Player.Tile.Position, fromTile.Position);

                                if (moveDirections.Length == 0)
                                {
                                    context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNoWay) );
                                }
                                else
                                {
                                    WalkToCommand command = new WalkToCommand(Player, moveDirections);

                                    command.Completed += (s, e) =>
                                    {
                                        e.Server.QueueForExecution(Constants.PlayerSchedulerEvent(Player), Constants.PlayerItemUseWithDelay, this);
                                    };

                                    command.Execute(server, context);
                                }                       
                            }
                            else
                            {
                                if ( fromItem.Metadata.Flags.Is(ItemMetadataFlags.Useable) )
                                {
                                    ItemUseWithItemScript script;

                                    if ( !server.ItemUseWithItemScripts.TryGetValue(fromItem.Metadata.OpenTibiaId, out script) || !script.Execute(Player, fromItem, toItem, server, context) )
                                    {
                                        context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisItem) );
                                    }
                                    else
                                    {
                                        //Act

                                        base.Execute(server, context);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}