using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Scripts;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithCreatureCommand : Command
    {
        public UseItemWithCreatureCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        protected bool IsNextTo(Tile fromTile, Server server, CommandContext context)
        {
            if ( !Player.Tile.Position.IsNextTo(fromTile.Position) )
            {
                WalkToUnknownPathCommand walkToUnknownPathCommand = new WalkToUnknownPathCommand(Player, fromTile);

                walkToUnknownPathCommand.Completed += (s, e) =>
                {
                    server.QueueForExecution(Constants.PlayerSchedulerEvent(Player), Constants.PlayerSchedulerEventDelay, this);
                };

                walkToUnknownPathCommand.Execute(server, context);

                return false;
            }

            return true;
        }

        protected void UseItemWithCreature(Item fromItem, Creature toCreature, Server server, CommandContext context, Action howToProceed)
        {
            ItemUseWithCreatureScript script;

            if ( !server.ItemUseWithCreatureScripts.TryGetValue(fromItem.Metadata.OpenTibiaId, out script) )
            {
                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisItem) );
            }
            else
            {
                bool proceed = true;

                if (script.NextTo)
                {
                    if ( !Player.Tile.Position.IsNextTo(toCreature.Tile.Position) )
                    {
                        howToProceed();

                        proceed = false;
                    }
                }
                else
                {
                    if ( !server.Pathfinding.CanThrow(Player.Tile.Position, toCreature.Tile.Position) )
                    {
                        context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThere) );

                        proceed = false;
                    }
                }

                if (proceed)
                {
                    if ( !script.Execute(Player, fromItem, toCreature, server, context) )
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
    }
}