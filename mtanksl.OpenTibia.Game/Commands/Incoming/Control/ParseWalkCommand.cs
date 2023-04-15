﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseWalkCommand : Command
    {
        public ParseWalkCommand(Player player, MoveDirection moveDirection)
        {
            Player = player;

            MoveDirection = moveDirection;
        }

        public Player Player { get; set; }

        public MoveDirection MoveDirection { get; set; }

        public override Promise Execute()
        {
            Tile toTile = Context.Server.Map.GetTile(Player.Tile.Position.Offset(MoveDirection) );

            if (toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) || toTile.GetCreatures().Any(c => c.Block) )
            {
                Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible),

                                                            new StopWalkOutgoingPacket(Player.Direction) );

                return Promise.Break;
            }

            return Promise.Delay(Constants.PlayerWalkSchedulerEvent(Player), 1000 * toTile.Ground.Metadata.Speed / Player.Speed).Then( () =>
            {
                Tile toTile = Context.Server.Map.GetTile(Player.Tile.Position.Offset(MoveDirection) );

                if (toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) || toTile.GetCreatures().Any(c => c.Block) )
                {
                    Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible),

                                                                new StopWalkOutgoingPacket(Player.Direction) );

                    return Promise.Break;
                }

                return Context.AddCommand(new CreatureUpdateParentCommand(Player, toTile) );
            } );
        }
    }
}