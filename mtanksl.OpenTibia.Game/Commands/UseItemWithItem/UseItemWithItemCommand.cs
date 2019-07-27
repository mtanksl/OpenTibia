using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Scripts;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public abstract class UseItemWithItemCommand : Command
    {
        public UseItemWithItemCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        protected void UseItemWithItem(Item fromItem, Item toItem, Server server, CommandContext context)
        {
            ItemUseWithItemScript script;

            if ( !server.ItemUseWithItemScripts.TryGetValue(fromItem.Metadata.OpenTibiaId, out script) )
            {
                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisItem) );
            }
            else
            {
                if ( !script.Execute(Player, fromItem, toItem, server, context) )
                {
                    context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisItem));
                }
                else
                {
                    base.Execute(server, context);
                }
            }
        }

        protected void UseItemWithItem(Item fromItem, Item toItem, Tile toTile, Server server, CommandContext context, Action howToProceed)
        {
            ItemUseWithItemScript script;

            if ( !server.ItemUseWithItemScripts.TryGetValue(fromItem.Metadata.OpenTibiaId, out script) )
            {
                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisItem) );
            }
            else
            {
                bool proceed = true;

                if (script.NextTo)
                {
                    if ( !Player.Tile.Position.IsNextTo(toTile.Position) )
                    {
                        howToProceed();

                        proceed = false;
                    }
                }
                else
                {
                    if ( !server.Pathfinding.CanThrow(Player.Tile.Position, toTile.Position) )
                    {
                        context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThere) );

                        proceed = false;
                    }
                }

                if (proceed)
                {
                    if ( !script.Execute(Player, fromItem, toItem, server, context) )
                    {
                        context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisItem));
                    }
                    else
                    {
                        base.Execute(server, context);
                    }
                }
            }
        }
    }
}