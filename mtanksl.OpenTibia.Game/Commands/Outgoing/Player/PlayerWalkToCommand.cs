using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class PlayerWalkToCommand : IncomingCommand
    {
        public PlayerWalkToCommand(Player player, Tile tile)
        {
            Player = player;

            Tile = tile;
        }

        public Player Player { get; set; }

        public Tile Tile { get; set; }

        public override async Promise Execute()
        {
            MoveDirection[] moveDirections = Context.Server.Pathfinding.GetMoveDirections(Player.Tile.Position, Tile.Position);

            if (moveDirections.Length == 0)
            {
                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNoWay) );

                await Promise.Break;
            }

            foreach (var moveDirection in moveDirections)
            {
                Tile fromTile = Player.Tile;

                Tile toTile = Context.Server.Map.GetTile(fromTile.Position.Offset(moveDirection) );

                if (toTile == null || toTile.Ground == null || toTile.NotWalkable || toTile.Block)
                {
                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );

                    Context.AddPacket(Player, new StopWalkOutgoingPacket(Player.Direction) );

                    await Promise.Break;
                }

                await Context.AddCommand(new CreatureMoveCommand(Player, toTile) );

                await Context.Server.GameObjectComponents.AddComponent(Player, new PlayerActionDelayBehaviour(TimeSpan.FromMilliseconds(fromTile.Position.ToDiagonalCost(toTile.Position) * 1000 * toTile.Ground.Metadata.Speed / Player.Speed) ) ).Promise;
            }
        }
    }
}