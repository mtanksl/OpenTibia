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

        protected bool IsUseable(Item fromItem, Context context)
        {
            if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Useable) )
            {
                return false;
            }

            return true;
        }

        protected void UseItemWithCreature(Item fromItem, Creature toCreature, Context context, Action howToProceed)
        {
            IItemUseWithCreatureScript script;

            if ( !context.Server.Scripts.ItemUseWithCreatureScripts.TryGetValue(fromItem.Metadata.OpenTibiaId, out script) )
            {
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisItem) );
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
                    if ( !context.Server.Pathfinding.CanThrow(Player.Tile.Position, toCreature.Tile.Position) )
                    {
                        context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThere) );

                        proceed = false;
                    }
                }

                if (proceed)
                {
                    if ( !script.OnItemUseWithCreature(Player, fromItem, toCreature, context) )
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
    }
}