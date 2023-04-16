﻿using OpenTibia.Common.Objects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerDestroyCommand : Command
    {
        public PlayerDestroyCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            Tile fromTile = Player.Tile;

            return Context.AddCommand(new TileRemoveCreatureCommand(fromTile, Player) ).Then( () =>
            {
                if (Player.Health == 0)
                {
                    Context.AddPacket(Player.Client.Connection, new OpenYouAreDeathDialogOutgoingPacket() );
                }
                else
                {
                    Context.Disconnect(Player.Client.Connection);
                }

                Context.Server.PlayerFactory.Destroy(Player);

                Context.AddEvent(new PlayerLogoutEventArgs(Player) );

                return Promise.Completed;
            } );
        }
    }
}