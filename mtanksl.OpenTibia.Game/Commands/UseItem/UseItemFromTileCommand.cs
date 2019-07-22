﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class UseItemFromTileCommand : UseItemCommand
    {
        public UseItemFromTileCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    if ( !Player.Tile.Position.IsNextTo(fromTile.Position) )
                    {
                        MoveDirection[] moveDirections = server.Pathfinding.Walk(Player.Tile.Position, fromTile.Position);

                        if (moveDirections.Length == 0)
                        {
                            context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNoWay) );
                        }
                        else
                        {
                            WalkToCommand command = new WalkToCommand(Player, moveDirections);

                            command.Completed += (s, e) =>
                            {
                                Execute(e.Server, e.Context);
                            };

                            command.Execute(server, context);
                        }                       
                    }
                    else
                    {
                        //Act

                        Container container = fromItem as Container;

                        if (container != null)
                        {
                            OpenOrCloseContainer(Player, container, server, context);
                        }

                        base.Execute(server, context);
                    }
                }
            }
        }
    }
}