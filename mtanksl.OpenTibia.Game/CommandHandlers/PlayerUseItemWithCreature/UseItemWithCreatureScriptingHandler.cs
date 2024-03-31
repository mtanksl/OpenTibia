using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Plugins;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWithCreatureScriptingHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private bool allowFarUse;

        public UseItemWithCreatureScriptingHandler(bool allowFarUse)
        {
            this.allowFarUse = allowFarUse;
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            PlayerUseItemWithCreaturePlugin plugin = Context.Server.Plugins.GetPlayerUseItemWithCreaturePlugin(allowFarUse, command.Item.Metadata.OpenTibiaId);

            if (plugin != null)
            {
                if ( !Context.Server.Pathfinding.CanThrow(command.Player.Tile.Position, command.ToCreature.Tile.Position) )
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThere) );

                    return Promise.Break;
                }

                return plugin.OnUseItemWithCreature(command.Player, command.Item, command.ToCreature).Then( (result) =>
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