using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class CreatureWalkToCommand : IncomingCommand
    {
        public CreatureWalkToCommand(Creature creature, Tile tile)
        {
            Creature = creature;

            Tile = tile;
        }

        public Creature Creature { get; set; }

        public Tile Tile { get; set; }

        public override async Promise Execute()
        {
            MoveDirection[] moveDirections = Context.Server.Pathfinding.GetMoveDirections(Creature.Tile.Position, Tile.Position, true);

            if (moveDirections.Length == 0)
            {
                if (Creature is Player player)
                {
                    Context.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNoWay) );
                }

                await Promise.Break;
            }

            for (int i = 0; i < moveDirections.Length; i++)
            {
                MoveDirection moveDirection = moveDirections[i];

                Tile fromTile = Creature.Tile;

                Tile toTile = Context.Server.Map.GetTile(fromTile.Position.Offset(moveDirection) );

                if (toTile == null || toTile.Ground == null || toTile.NotWalkable || toTile.Block)
                {
                    if (Creature is Player player)
                    {
                        Context.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );

                        Context.AddPacket(player, new StopWalkOutgoingPacket(player.Direction) );
                    }

                    await Promise.Break;
                }

                int diagonalCost = (moveDirection == MoveDirection.NorthWest || moveDirection == MoveDirection.NorthEast || moveDirection == MoveDirection.SouthWest || moveDirection == MoveDirection.SouthEast)? 2 : 1;

                await Context.Server.GameObjectComponents.AddComponent(Creature, new PlayerWalkDelayBehaviour(TimeSpan.FromMilliseconds(diagonalCost * 1000 * toTile.Ground.Metadata.Speed / Creature.Speed) ) ).Promise;
                
                if (toTile.NotWalkable || toTile.Block)
                {
                    if (Creature is Player player)
                    {
                        Context.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );

                        Context.AddPacket(player, new StopWalkOutgoingPacket(player.Direction) );
                    }

                    await Promise.Break;
                }

                await Context.AddCommand(new CreatureMoveCommand(Creature, toTile) );

                if (i == moveDirections.Length - 1)
                {
                    await Context.Server.GameObjectComponents.AddComponent(Creature, new PlayerWalkDelayBehaviour(TimeSpan.FromMilliseconds(diagonalCost * 1000 * toTile.Ground.Metadata.Speed / Creature.Speed) ) ).Promise;
                }
            }
        }
    }
}