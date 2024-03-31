using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Plugins;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWithItemScriptingHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private bool allowFarUse;

        public UseItemWithItemScriptingHandler(bool allowFarUse)
        {
            this.allowFarUse = allowFarUse;
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            PlayerUseItemWithItemPlugin plugin = Context.Server.Plugins.GetPlayerUseItemWithItemPlugin(allowFarUse, command.Item.Metadata.OpenTibiaId);

            if (plugin != null)
            {
                if (command.ToItem.Parent is Tile toTile && !Context.Server.Pathfinding.CanThrow(command.Player.Tile.Position, toTile.Position) )
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThere) );

                    return Promise.Break;
                }

                return plugin.OnUseItemWithItem(command.Player, command.Item, command.ToItem).Then( (result) =>
                {
                    if (result)
                    {
                        return Promise.Completed;
                    }

                    return next();
                } );
            }

            return next();
        }
    }
}