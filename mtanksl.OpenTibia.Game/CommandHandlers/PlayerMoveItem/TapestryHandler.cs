using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class TapestryHandler : CommandHandler<PlayerMoveItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.Item.Metadata.Flags.Is(ItemMetadataFlags.Hangable) && command.ToContainer is Tile toTile)
            {
                bool hasHangableItem = false;

                bool? vertical = null;

                foreach (var item in toTile.GetItems() )
                {
                    if (item.Metadata.Flags.Is(ItemMetadataFlags.Hangable) )
                    {
                        hasHangableItem = true;
                    } 

                    if (item.Metadata.Flags.Is(ItemMetadataFlags.Vertical) )
                    {
                        if (vertical == null)
                        {
                            vertical = true;
                        }
                    }

                    if (item.Metadata.Flags.Is(ItemMetadataFlags.Horizontal) )
                    {
                        if (vertical == null)
                        {
                            vertical = false;
                        }
                    }
                }

                if ( !hasHangableItem)
                {
                    if (vertical == true)
                    {
                        if (command.Player.Tile.Position.X < toTile.Position.X)
                        {
                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotThrowThere) );

                            return Promise.Break;
                        }

                        return command.Execute();
                    }
                    else if (vertical == false)
                    {
                        if (command.Player.Tile.Position.Y < toTile.Position.Y)
                        {
                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotThrowThere) );

                            return Promise.Break;
                        }

                        return command.Execute();
                    }
                }
            }

            return next();
        }
    }
}